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
						s.Start();
						while (s.status != enumStatus.Completed && !this.stopping && (this.treeview.Parent != null))
						{
							UpdateStatus();
							Thread.Sleep(500);
						}
						s.progress = 100;
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
				TreeNode node = main.Nodes.Add(s.Name, s.Name + " [" + s.status + "]" + " [" + s.progress + "]");
				foreach (OneThread thr in s.Threads)
				{
					node.Nodes.Add(thr.name, thr.name + " [" + (thr.status == ThreadState.Stopped ? "Completed" : thr.status.ToString()) + "]" + " [" + thr.progress + "]");
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
				TreeNode node = new TreeNode(s.Name + " [" + s.status + "]" + " [" + s.progress + "]");
				foreach (OneThread thr in s.Threads)
				{
					node.Nodes.Add(thr.name, thr.name + " [" + (thr.status == ThreadState.Stopped ? "Completed" : thr.status.ToString()) + "]" + " [" + thr.progress + "]");
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
		public ThreadManager manager;
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
						s.progress = 100;
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
				if (this.Threads.Length > 0)
				{
					foreach (OneThread thr in this.Threads)
					{
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

		
	}
	public class OneThread
	{
		public DateTime startdate;
		public DateTime stopdate;
		public Stage parent;
		private Thread thread;
		private Action<OneThread> method;
		public EventHandler Completed;
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
			if (thread.ThreadState == ThreadState.Unstarted){
				this.thread.Start();
				this.startdate = DateTime.Now;
				new Thread(stopwaiting).Start();
			}
		}

		public void stopwaiting()
		{
			while(this.thread.ThreadState != ThreadState.Stopped && !this.stopping )
			{
				Thread.Sleep(500);
			}
			this.parent.LastCompletedThread = this;
			this.stopdate = DateTime.Now;
			Completed(this, new EventArgs());
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

		public void Stop()
		{
			this.stopping = true;
		}
	}
}
