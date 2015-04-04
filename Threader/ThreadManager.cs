using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Threader
{
	public enum enumStatus { Unstarted, Started, Paused, Completed, Unknown, Running }
	public class ThreadManager : Stage
	{
		private bool stopping = false;
		private Thread main;
		private List<Stage> dict = new List<Stage>();

		public EventHandler Completed;
		public EventHandler ThreadCompleted;
		public EventHandler ThreadStarting;
		private RichTextBox textbox;
		private TreeView treeview;
		public ThreadManager(List<Stage> argdict, RichTextBox argtextbox, TreeView statustreeview)
		{
			this.Name = "ThreadManager";
			this.textbox = argtextbox;
			this.treeview = statustreeview;
			this.dict = argdict;
			foreach (Stage s in this.dict)
			{
				string sname = s.Name.ToString();
				s.manager = this;
				s.parent = this;
				foreach(OneThread thr in s.Threads)
				{
					thr.manager = this;
				}
				this.treeview.Nodes.Add(sname, sname);
				this.UpdateStatus();
			}

			this.treeview.ExpandAll();
		}

		public void Abort()
		{
			this.stopping = true;
			foreach(Stage s in this.dict)
			{
				_Abort(s);
			}
		}
		private void _Abort(Stage stage)
		{
			stage.Stop();
			foreach (Stage s in stage.SubStages)
			{
				s.Stop();
			}
		}
		public new void Start()
		{
			ResetStatus(this.dict.ToArray());
			UpdateStatus();
			main = new Thread(delegate()
			{
				Clear();
				foreach (Stage s in this.dict)
				{
					if (!this.stopping)
					{
						Console.WriteLine("MAN: start " + s.Name);
						s.Start();
						while (s.status != enumStatus.Completed && !this.stopping && (this.treeview.Parent != null))
						{
							UpdateStatus();
							Thread.Sleep(500);
						}
						s.progress = 101;
						Console.WriteLine("MAN: complet " + s.Name);
						this.LastStageCompleted = s;
					}
				}
				if (!this.stopping)
				{
					UpdateStatus();
					Completed(this,new EventArgs());
				}
			});
			main.Start();
		}

		private void ResetStatus(Stage[] list)
		{
			foreach (Stage s in list)
			{
				foreach (OneThread thr in s.Threads)
				{
					thr.Reset();
				}
				s.progress = 0;
				ResetStatus(s.SubStages);
			}
		}

		private TreeView GetList(Stage[] list)
		{
			TreeView main = new TreeView();
			foreach (Stage s in list)
			{
				TreeNode node = main.Nodes.Add(s.Name, s.Name + " [" + s.status + "]" + " [" + s.getprogress() + "]");
				foreach (OneThread thr in s.Threads)
				{
					node.Nodes.Add(thr.name, thr.name + " [" + (thr.status == ThreadState.Stopped ? "Completed" : thr.status.ToString()) + "]" + " [" + thr.getprogress() + "]");
				}
				foreach (TreeNode tr in this._GetList(s.SubStages))
				{
					node.Nodes.Add(tr);
				}
			}
			return main;
		}

		private TreeNode[] _GetList(Stage[] list)
		{
			List<TreeNode> array = new List<TreeNode>();
			foreach (Stage s in list)
			{
				TreeNode node = new TreeNode(s.Name + " [" + s.status + "]" + " [" + s.getprogress() + "]");
				foreach (OneThread thr in s.Threads)
				{
					node.Nodes.Add(thr.name, thr.name + " [" + (thr.status == ThreadState.Stopped ? "Completed" : thr.status.ToString()) + "]" + " [" + thr.getprogress() + "]");
				}
				foreach (TreeNode tr in this._GetList(s.SubStages))
				{
					node.Nodes.Add(tr);
				}
				array.Add(node);
			}
			return array.ToArray();
		}

		public void UpdateStatus()
		{
			TreeView collect = this.GetList(this.dict.ToArray());
			if (!this.stopping && (this.treeview.Parent != null))
			{
				try
				{
					this.treeview.Invoke((MethodInvoker)delegate()
					{
						if (!this.stopping && (this.treeview.Parent != null))
						{
							this.treeview.Nodes.Clear();
						}

						foreach (TreeNode node in collect.Nodes)
						{
							if (!this.stopping && (this.treeview.Parent != null))
							{
								this.treeview.Nodes.Add((TreeNode)node.Clone());
							}
						}
						if (!this.stopping && (this.treeview.Parent != null))
						{
							this.treeview.ExpandAll();
						}
					});
				}
				catch
				{ }

			}
		}

		public void Print(string Data)
		{
			try
			{
				this.textbox.Invoke((MethodInvoker)delegate()
				{
					this.textbox.AppendText("T:" + Data + "\n");
				});
			}
			catch { }
		}

		public bool Working()
		{
			//bool ret = this.main != null && (_working(this.dict.ToArray()) || this.main.ThreadState != ThreadState.Stopped && this.main.ThreadState != ThreadState.Unstarted);
			//Print("work" + ret.ToString() + "\n");
			return _working(this.dict.ToArray());
		}

		private bool _working(Stage[] array)
		{
			bool total=false;
			foreach (Stage s in array)
			{
				bool ww = false;
				bool st = false;
				if (s.Threads.Length > 0)
				{
					foreach (OneThread thr in s.Threads)
					{
						if (thr.status != ThreadState.Stopped && thr.status != ThreadState.Unstarted)
						{
							//Print(thr.name + " " + thr.status + "\n");
							ww = true;
						}
					}
				}
				if (s.SubStages.Length>0)
				{
					st = _working(s.SubStages);
				}
				total = total || ww || st;
			}
			return total;
		}

		public void Clear()
		{
			try
			{
				this.textbox.Invoke((MethodInvoker)delegate()
				{
					this.textbox.Text = "";
				});
			}
			catch { }
		}
	}

	public class Stage
	{
		public OneThread[] Threads = new OneThread[0];
		public string Name;
		public Stage[] SubStages = new Stage[0];
		public OneThread LastCompletedThread;
		public Stage LastStageCompleted;
		public Stage parent;
		private ThreadManager _manager;
		public ThreadManager manager
		{
			set
			{
				_manager = value;
				foreach (OneThread thr in this.Threads)
				{
					thr.manager = _manager;
				}
				foreach (Stage sub in this.SubStages)
				{
					sub.manager = _manager;
				}
			}
			get
			{
				return _manager;
			}
		}
		private bool stopping = false;

		public Stage()
		{ }
		public Stage(string name, OneThread[] thrs)
		{
			this.Name = name;
			this.Threads = thrs;
			foreach (OneThread thr in this.Threads)
			{
				thr.parent = this;
			}
		}
		public Stage(string name, OneThread[] thrs, Stage[] subs)
		{
			this.Name = name;
			this.Threads = thrs;
			this.SubStages = subs;
			foreach (OneThread thr in this.Threads)
			{
				thr.parent = this;
			}
			foreach (Stage sub in this.SubStages)
			{
				sub.parent = this;
			}
		}

		public void Start()
		{
			if (!this.stopping)
			{
				foreach (OneThread thr in this.Threads)
				{
					if (!this.stopping)
					{
						thr.Start();
					}
				}
				foreach (Stage s in this.SubStages)
				{
					if (!this.stopping)
					{
						s.Start();
						while (s.status != enumStatus.Completed && !this.stopping)
						{
							this.manager.UpdateStatus();
							Thread.Sleep(500);
						}
						s.progress = 101;
						this.LastStageCompleted = s;
					}
				}
			}
		}

		public void Stop()
		{
			this.stopping = true;
			foreach (OneThread thr in this.Threads)
			{
				thr.Stop();
			}
		}
		public int progress
		{
			get
			{
				int totaltasks = SubStages.Length*2 + Threads.Length;
				int totalprog = 0;
				for (int i = 0; i < SubStages.Length; i++)
				{
					totalprog += SubStages[i].progress*2;
				}
				for (int i = 0; i < Threads.Length; i++)
				{
					totalprog += Threads[i].progress;
				}
				return totalprog / totaltasks;
			}
			set
			{
				for (int i = 0; i < SubStages.Length; i++)
				{
					SubStages[i].progress = value;
				}
				for (int i = 0; i < Threads.Length; i++)
				{
					Threads[i].progress = value;
				}
			}
		}
		public enumStatus status
		{
			get
			{
				bool created = true;
				bool paused = true;
				bool started = true;
				bool completed = true;
				bool running = false;
				int i = 0;
				foreach (OneThread thr in this.Threads)
				{
					i += thr.progress;
					if (thr.status != ThreadState.Unstarted)
					{
						created = false;
					}
					if (thr.status != ThreadState.Running)
					{
						started = false;
						running = true;
					}
					if (thr.status != ThreadState.Suspended)
					{
						paused = false;
					}
					if (thr.status != ThreadState.Stopped)
					{
						completed = false;
					}
				}
				if (this.SubStages.Length > 0)
				{
					foreach (Stage sub in this.SubStages)
					{
						if (sub.status != enumStatus.Completed)
						{
							completed = false;
						}
						if (sub.status != enumStatus.Unstarted)
						{
							created = false;
						}
						if (sub.status != enumStatus.Paused)
						{
							paused = false;
						}
						if (sub.status != enumStatus.Started)
						{
							started = false;
							running = true;
						}
					}
				}
				if (this.Name == "INIT")
				{
					Console.WriteLine("PROG [" + (i / Threads.Length) + "] " + this.Name);
				}
				if ((i / Threads.Length) != 101)
				{
					return enumStatus.Running;
				}
				if (created)
				{
					return enumStatus.Unstarted;
				}
				if (paused)
				{
					return enumStatus.Paused;
				}
				if (completed)
				{
					return enumStatus.Completed;
				}
				if (started)
				{
					return enumStatus.Started;
				}
				if (running)
				{
					return enumStatus.Running;
				}
				else
				{
					return enumStatus.Unknown;
				}
			}
		}

		

		public int getprogress()
		{
			int i = this.progress;
			if (i == 101)
			{
				return 100;
			}
			else
			{
				return i;
			}
		}

		
	}
	public class OneThread
	{
		public DateTime startdate;
		public ThreadManager manager;
		public DateTime stopdate;
		public Stage parent;
		private Thread thread;
		private Action<OneThread> method;
		public int progress = 0;
		private bool stopping = false;
		public string name { get; set; }
		public ThreadState status
		{
			get
			{
				if (thread.ThreadState == ThreadState.WaitSleepJoin)
					return ThreadState.Running;
				else
					return thread.ThreadState;
			}
		}

		public OneThread(string argname, Action<OneThread> argmethod)
		{
			this.name = argname;
			this.method = argmethod;
			this.thread = new Thread(delegate(){
				if (!this.stopping)
				{
					try { this.method(this); }
					catch { }
				}
			});
		}

		public void Start()
		{
			if (thread.ThreadState == ThreadState.Unstarted)
			{
				this.thread.Start();
				this.startdate = DateTime.Now;
				this.manager.ThreadStarting(this, new EventArgs());
				new Thread(stopwaiting).Start();
			}
		}

		public void stopwaiting()
		{
			Console.WriteLine(this.name+" "+this.thread.ThreadState + " " + (this.thread.ThreadState != ThreadState.Stopped )+ "\n");
			while (this.thread.ThreadState != ThreadState.Stopped && this.thread.ThreadState != ThreadState.Unstarted)
			{
				Thread.Sleep(500);
			}
			this.parent.LastCompletedThread = this;
			this.stopdate = DateTime.Now;
			Console.WriteLine(this.parent.status+" "+this.name+" Acompleted"+ "\n");
			this.manager.ThreadCompleted(this, new EventArgs());
			this.progress = 101;
		}

		public void Reset()
		{
			if (thread.ThreadState == ThreadState.Stopped)
			{
				this.thread = new Thread(delegate()
				{
					if (!this.stopping)
					{
						try { this.method(this); }
						catch { }
					}
				});
				this.startdate = new DateTime();
				this.progress = 0;
			}
		}

		public int getprogress()
		{
			int i = this.progress;
			if (i == 101)
			{
				return 100;
			}else
			{
				return i;
			}
		}

		public string getlastthread()
		{
			return _getlastthread(this.parent);
			//if (this.parent.LastStageCompleted)
			//{
			//	Console.WriteLine("main : " + thr.parent.parent.Name);
			//	if (thr.parent.parent.LastStageCompleted != null)
			//		if (thr.parent.parent.LastStageCompleted.LastCompletedThread != null)
			//		{
			//			Console.WriteLine("thred : " + thr.parent.parent.LastStageCompleted.LastCompletedThread.name);
			//			lastthr = thr.parent.parent.LastStageCompleted.LastCompletedThread.name;
			//		}
			//		else
			//			Console.WriteLine("no thred : ");
			//}
		}

		private string _getlastthread(Stage st)
		{
			if (st.parent != null)
			{
				if (st.parent.LastStageCompleted != null)
				{
					if(st.parent.LastStageCompleted.SubStages.Length > 0)
					{
						Console.WriteLine("getdownthread");
						return getdownthread(st.parent);
					}
					else
					{
						return st.parent.LastStageCompleted.LastCompletedThread.name;
					}
					//return st.parent.LastStageCompleted.LastCompletedThread.name;
				}
				else
				{
					return _getlastthread(st.parent);
				}
			}
			else
			{
				if (st == this.manager)
				{
					if (st.LastStageCompleted != null)
					{
						if (st.LastStageCompleted.SubStages.Length > 0)
						{
							return getdownthread(st);
						}
						else
						{
							return st.LastStageCompleted.LastCompletedThread.name;
						}
					}
					else
					{
						return "User";
					}
				}
				else
				{
					return "Error";
				}
			}
		}
		private string getdownthread(Stage st)
		{
			if (st.LastStageCompleted.SubStages.Length > 0)
			{
				Stage stw = _getdownthread(st.LastStageCompleted);
				if (st.LastCompletedThread != null)
				{
					if (stw.LastCompletedThread.stopdate >= st.LastCompletedThread.stopdate)
					{
						return stw.LastCompletedThread.name;
					}
					else
					{
						return st.LastCompletedThread.name;
					}
				}
				else
				{
					return stw.LastCompletedThread.name;
				}
			}
			else
			{
				return st.LastCompletedThread.name;
			}
		}

		private Stage _getdownthread(Stage st)
		{
			if (st.LastStageCompleted.SubStages.Length > 0)
			{
				return _getdownthread(st.LastStageCompleted);
			}else
			{
				return st.LastStageCompleted;
			}
		}

		public void Stop()
		{
			this.stopping = true;
		}
	}
}
