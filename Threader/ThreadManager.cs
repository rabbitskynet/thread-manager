using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Threader
{
	enum enumOneThread { A, B, C, D, E, F, G, H, K }
	enum enumStages { INIT, MAIN, FINISH }
	enum enumSubStages { WORK, SUB1, SUB2, SUB3 }
	enum enumStatus { Unstarted, Started, Paused, Completed, Unknown }
	class ThreadManager
	{
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
				this.treeview.Nodes.Add(sname, sname);
				foreach (SubStage sub in s.SubStages)
				{
					string subname = sub.Name.ToString();
					this.treeview.Nodes[sname].Nodes.Add(subname, subname);
					foreach (OneThread act in sub.Threads)
					{
						string actname = act.name.ToString();
						this.treeview.Nodes[sname].Nodes[subname].Nodes.Add(actname, actname);
					}
				}
			}
			this.treeview.ExpandAll();
		}

		public void Start()
		{
			ResetStatus();
			UpdateStatus();
			Thread main = new Thread(delegate()
			{
				Clear();
				foreach (Stage s in this.dict)
				{
					s.Start();
					while (s.status != enumStatus.Completed)
					{
						UpdateStatus();
						Print("STAGE: "+s.Name+" "+s.status.ToString());
						foreach(SubStage sub in s.SubStages)
						{
							Print("SUBSTG: "+sub.Name+" "+sub.status.ToString());
						}
						Thread.Sleep(1000);
					}
				}
				UpdateStatus();
			});
			main.Start();
		}

		private void ResetStatus()
		{
			foreach (Stage s in this.dict)
			{
				foreach (SubStage sub in s.SubStages)
				{
					foreach (OneThread thr in sub.Threads)
					{
						thr.Reset();
					}
				}
			}
		}

		private void UpdateStatus()
		{
			TreeView collect = new TreeView();
			foreach (Stage s in this.dict)
			{
				string sname = s.Name.ToString();
				collect.Nodes.Add(sname, sname + " [" + s.status + "]");
				foreach (SubStage sub in s.SubStages)
				{
					string subname = sub.Name.ToString();
					collect.Nodes[sname].Nodes.Add(subname, subname + " [" + sub.status + "]");
					foreach (OneThread act in sub.Threads)
					{
						string actname = act.name.ToString();
						collect.Nodes[sname].Nodes[subname].Nodes.Add(actname, actname + " [" + act.status.ToString() + "]");
					}
				}
			}

			lock (this.treeview)
			{
				this.treeview.Invoke((MethodInvoker)delegate()
				{
					this.treeview.Nodes.Clear();
					foreach (TreeNode node in collect.Nodes)
					{
						this.treeview.Nodes.Add(node.Name, node.Text);
						foreach (TreeNode subnode in node.Nodes)
						{
							this.treeview.Nodes[node.Name].Nodes.Add(subnode.Name, subnode.Text);
							foreach (TreeNode sub2node in subnode.Nodes)
							{
								this.treeview.Nodes[node.Name].Nodes[subnode.Name].Nodes.Add(sub2node.Name, sub2node.Text);
							}
						}
					}
					this.treeview.ExpandAll();
				});
			}


			//foreach (Stage s in this.dict)
			//{
			//	string sname = s.Name.ToString();
			//	this.treeview.Invoke((MethodInvoker)delegate()
			//	{
			//		this.treeview.Nodes[sname].Text = sname + " [" + s.status + "]";
			//	});
				
			//	foreach (SubStage sub in s.SubStages)
			//	{
			//		string subname = sub.Name.ToString();
			//		this.treeview.Invoke((MethodInvoker)delegate()
			//		{
			//			this.treeview.Nodes[sname].Nodes[subname].Text = subname + " [" + sub.status + "]";
			//		});
			//		foreach (OneThread act in sub.Threads)
			//		{
			//			string actname = act.name.ToString();
			//			this.treeview.Invoke((MethodInvoker)delegate()
			//			{
			//				this.treeview.Nodes[sname].Nodes[subname].Nodes[actname].Text = actname + " [" + act.status.ToString() + "]";
			//			});
			//			//this.treeview.Nodes[sname].Nodes[subname].Nodes[actname].Text = actname + " [" + act.status.ToString() + "]";
			//		}
			//	}
			//}
		}

		public void Print(string Data)
		{
			this.textbox.Invoke((MethodInvoker)delegate()
			{
				this.textbox.AppendText("T:" + Data + "\n");
			});
		}

		public void Clear()
		{
			this.textbox.Invoke((MethodInvoker)delegate()
			{
				this.textbox.Text="";
			});
		}
	}

	class SubStage
	{
		public enumSubStages Name;
		public OneThread[] Threads;

		public enumStatus status
		{
			get
			{
				bool created = true;
				bool paused = true;
				bool started = true;
				bool completed = true;
				bool running = false;
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
					return enumStatus.Started;
				}else
				{
					return enumStatus.Unknown;
				}
			}
		}
		public SubStage(enumSubStages name, OneThread[] threads)
		{
			this.Name = name;
			this.Threads = threads;
		}

		public void Start()
		{
			foreach (OneThread th in this.Threads)
			{
				th.Start();
			}
		}
	}

	class Stage
	{
		public enumStages Name;
		public SubStage[] SubStages;
		public enumStatus status
		{
			get
			{
				bool created = true;
				bool paused = true;
				bool started = true;
				bool completed = true;
				foreach (SubStage sub in this.SubStages)
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
				return enumStatus.Unknown;
			}
		}

		public void Start()
		{
			foreach (SubStage s in this.SubStages)
			{
				s.Start();
			}
		}
		public Stage(enumStages name, SubStage[] subs)
		{
			this.Name = name;
			this.SubStages = subs;
		}
	}
	class OneThread
	{
		private Thread thread;
		private Action method;
		public enumOneThread name { get; set; }
		public ThreadState status
		{
			get
			{
				return thread.ThreadState;
			}
		}

		public OneThread(enumOneThread argname, Action argmethod)
		{
			this.name = argname;
			this.method = argmethod;
			this.thread = new Thread(() => this.method());
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
				this.thread = new Thread(() => this.method());
			}
		}
	}
}
