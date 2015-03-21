using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Threader
{
	enum enumActions { A, B, C, D, E, F, G, H, K }
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
					foreach (Actions act in sub.Threads)
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
			UpdateStatus();
			Thread main = new Thread(delegate()
			{
				foreach (Stage s in this.dict)
				{
					//s.Start();
					while (s.status != enumStatus.Completed)
					{
						UpdateStatus();
					}
				}
			});
		}

		public void UpdateStatus()
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
					foreach (Actions act in sub.Threads)
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
			//		foreach (Actions act in sub.Threads)
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

		public void Worker(object Data)
		{
			for (int i = 0; i < 10; i++)
				this.textbox.Invoke((MethodInvoker)delegate()
				{
					this.textbox.AppendText(((int)Data).ToString() + "hi there "+i+" \n");
				});
		}
	}

	class SubStage
	{
		public enumSubStages Name;
		public Actions[] Threads;

		public enumStatus status
		{
			get
			{
				bool created = true;
				bool paused = true;
				bool started = true;
				bool completed = true;
				foreach (Actions thr in this.Threads)
				{
					if (thr.status != ThreadState.Unstarted)
					{
						created = false;
					}
					if (thr.status != ThreadState.Running)
					{
						started = false;
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
				return enumStatus.Unknown;
			}
		}
		public SubStage(enumSubStages name, Actions[] threads)
		{
			this.Name = name;
			this.Threads = threads;
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
		public Stage(enumStages name, SubStage[] subs)
		{
			this.Name = name;
			this.SubStages = subs;
		}
	}
	class Actions
	{
		private Thread thread;
		public enumActions name { get; set; }
		public ThreadState status
		{
			get
			{
				return thread.ThreadState;
			}
		}

		public Actions(enumActions argname, Action<Object> method)
		{
			this.name = argname;
			this.thread = new Thread((Object obj) => method(obj));
		}
		public void Start(Object Data)
		{
			if (thread.ThreadState == ThreadState.Unstarted){
				thread.Start(Data);
			}
		}
	}
}
