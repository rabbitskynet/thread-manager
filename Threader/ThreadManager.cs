using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Threader
{
	enum enumStatus { Unstarted, Started, Paused, Completed, Unknown, Running }
	class ThreadManager
	{
		private bool stopping = false;
		private Thread main;
		private List<Stage> dict = new List<Stage>();

		private RichTextBox textbox;
		private TreeView treeview;
		public ThreadManager(List<Stage> argdict, RichTextBox argtextbox, TreeView statustreeview)
		{
			this.textbox = argtextbox;
			this.treeview = statustreeview;
			this.dict = argdict;
			foreach (Stage s in this.dict)
			{
				string sname = s.Name.ToString();
				s.manager = this;
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
		public void Start()
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
						while (s.status != enumStatus.Completed && ! this.stopping)
						{
							UpdateStatus();
							Thread.Sleep(500);
						}
					}
				}
				UpdateStatus();
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
				ResetStatus(s.SubStages);
			}
		}

		private TreeView GetList(Stage[] list)
		{
			TreeView main = new TreeView();
			foreach (Stage s in list)
			{
				TreeNode node = main.Nodes.Add(s.Name, s.Name + " [" + s.status + "]");
				foreach (OneThread thr in s.Threads)
				{
					node.Nodes.Add(thr.name, thr.name + " [" + s.status + "]");
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
				TreeNode node = new TreeNode(s.Name);
				foreach (OneThread thr in s.Threads)
				{
					node.Nodes.Add(thr.name, thr.name + " [" + s.status + "]");
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
			if (!this.stopping)
			{
				this.treeview.Invoke((MethodInvoker)delegate()
				{
					this.treeview.Nodes.Clear();
					foreach (TreeNode node in collect.Nodes)
					{
						this.treeview.Nodes.Add((TreeNode)node.Clone());
					}
					this.treeview.ExpandAll();
				});

			}
		}

		public void Print(string Data)
		{
			this.textbox.Invoke((MethodInvoker)delegate()
			{
				this.textbox.AppendText("T:" + Data + "\n");
			});
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
			this.textbox.Invoke((MethodInvoker)delegate()
			{
				this.textbox.Text="";
			});
		}
	}

	class Stage
	{
		public OneThread[] Threads = new OneThread[0];
		public string Name;
		public Stage[] SubStages = new Stage[0];
		public ThreadManager manager;
		private bool stopping = false;
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
						while (s.status != enumStatus.Completed && ! this.stopping)
						{
							this.manager.UpdateStatus();
							Thread.Sleep(500);
						}
					}
				}
			}
		}
		public Stage(string name, OneThread[] thr)
		{
			this.Name = name;
			this.Threads = thr;
		}
		public Stage(string name, OneThread[] thr, Stage[] subs)
		{
			this.Name = name;
			this.Threads = thr;
			this.SubStages = subs;
		}

		public void Stop()
		{
			this.stopping = true;
			foreach (OneThread thr in this.Threads)
			{
				thr.Stop();
			}
		}
	}
	class OneThread
	{
		private Thread thread;
		private Action method;
		private bool stopping = false;
		public string name { get; set; }
		public ThreadState status
		{
			get
			{
				return thread.ThreadState;
			}
		}

		public OneThread(string argname, Action argmethod)
		{
			this.name = argname;
			this.method = argmethod;
			this.thread = new Thread(delegate(){
				if (!this.stopping)
				{
					try { this.method(); }
					catch { }
				}
			});
		}

		public void Start()
		{
			if (thread.ThreadState == ThreadState.Unstarted){
				this.thread.Start();
			}
		}

		public void Reset()
		{
			if (thread.ThreadState == ThreadState.Stopped)
			{
				this.thread = new Thread(delegate()
				{
					if (!this.stopping)
					{
						try { this.method(); }
						catch { }
					}
				});
			}
		}

		public void Stop()
		{
			this.stopping = true;
		}
	}
}
