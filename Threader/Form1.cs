using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;


namespace Threader
{
	enum enumOneThread { A, B, C, D, E, F, G, H, K }
	enum enumStages { INIT, MAIN, FINISH, SUB1, SUB2, SUB3 }
	public partial class Form1 : Form
	{
		private Form2 taskform;
		private ThreadManager manager;
		public Form1()
		{
			InitializeComponent();
			this.Icon = Threader.Properties.Resources.threadicon;
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			taskform = new Form2();

			List<Stage> dict = new List<Stage>();

			dict.Add(new Stage("INIT", new OneThread[]{
				new OneThread("A",(OneThread thr)=>generate(thr))
			}));
			dict.Add(new Stage("MAIN",new OneThread[]{
				new OneThread("B",(OneThread thr)=>f1(thr)),
				new OneThread("C",(OneThread thr)=>f2(thr))
			},
				new Stage[]{
					new Stage("SUB1",new OneThread[]{
						new OneThread("D",(OneThread thr)=>f3(thr))
				}),
					new Stage("SUB2",new OneThread[]{
						new OneThread("E",(OneThread thr)=>f4(thr)),
						new OneThread("F",(OneThread thr)=>f5(thr)),
						new OneThread("G",(OneThread thr)=>f6(thr))
				}),
					new Stage("SUB3",new OneThread[]{
						new OneThread("H",(OneThread thr)=>f7(thr))
				})
			}));
			dict.Add(new Stage("FINISH", new OneThread[]{
				new OneThread("K",(OneThread thr)=>f8(thr))
			}));

			manager = new ThreadManager(dict, this.textbox, this.statustreeview);
			manager.Completed += threadscompleted;
		}

		private void infobutton_Click(object sender, EventArgs e)
		{
			
		}

		private void taskbutton_Click(object sender, EventArgs e)
		{
			taskform.Show();
		}

		private void startbutton_Click(object sender, EventArgs e)
		{
			this.startbutton.Enabled = false;
			this.textbox.Text = "";
			this.manager.Start();
		}

		private void threadscompleted(object sender, EventArgs e)
		{
			try
			{
				this.Invoke((MethodInvoker)delegate()
				{
					if (this != null)
						this.startbutton.Enabled = true;
				});
			}
			catch { }
		}

		private void savebutton_Click(object sender, EventArgs e)
		{
			
		}

		public void write(string s)
		{
			this.textbox.Invoke((MethodInvoker)delegate()
			{
				this.textbox.AppendText(s);
			});
		}
		public void writeln(string s)
		{
			this.textbox.Invoke((MethodInvoker)delegate()
			{
				this.textbox.AppendText(s+"\n");
			});
		}

		private List<int[]> MX = new List<int[]>();

		public void generate(OneThread thr)
		{
			thr.progress = 0;
			Random r = new Random();
			int[] a = new int[10], b = new int[10], c = new int[10];
			for (int i = 0; i < 10; i++)
			{
				a[i] = r.Next(10) * i;
				b[i] = r.Next(10) * i;
				c[i] = r.Next(10) * i;
				thr.progress += 7;
				Thread.Sleep(500);
			}
			lock (MX)
			{
				MX.Add(a);
				MX.Add(b);
				MX.Add(c);
				thr.progress += 10;
				Thread.Sleep(500);
			}
			thr.progress = 100;
			write("M1 [");
			for (int i = 0; i < 10; i++)	
				write(a[i]+(i<9?",":""));
			writeln("]");
			write("M2 [");
			for (int i = 0; i < 10; i++)	
				write(b[i]+(i<9?",":""));
			writeln("]");
			write("M3 [");
			for (int i = 0; i < 10; i++)
			{
				write(c[i] + (i < 9 ? "," : ""));
			}
			writeln("]");
		}
		public void f1(OneThread thr)
		{
			List<int[]> arr;
			lock (MX)
			{
				arr = MX.Clone();
			}
			this.Invoke((MethodInvoker)delegate()
			{
				this.textbox.AppendText("f1: completed \n");
			});
		}
		public void f2(OneThread thr)
		{
			List<int[]> arr;
			lock (MX)
			{
				arr = MX.Clone();
			}
			this.textbox.Invoke((MethodInvoker)delegate()
			{
				this.textbox.AppendText("f2: completed \n");
			});
		}
		public void f3(OneThread thr)
		{
			List<int[]> arr;
			lock (MX)
			{
				arr = MX.Clone();
			}
			this.Invoke((MethodInvoker)delegate()
			{
				this.textbox.AppendText("f3: completed \n");
			});
		}
		public void f4(OneThread thr)
		{
			Thread.Sleep(3000);
		}
		public void f5(OneThread thr)
		{
			Thread.Sleep(3000);
		}
		public void f6(OneThread thr)
		{
			Thread.Sleep(3000);
		}
		public void f7(OneThread thr)
		{
			Thread.Sleep(3000);
		}
		public void f8(OneThread thr)
		{
			Thread.Sleep(3000);
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.manager.Working())
			{
				this.manager.Abort();
				Console.WriteLine(Monitor.TryEnter(textbox));
				//this.textbox.AppendText("status" + this.manager.Working().ToString() + "\n");
				while (this.manager.Working())
				{
					//this.textbox.AppendText("still working\n");
					Thread.Sleep(1000);
				}
				Thread.Sleep(1000);
			}
			this.taskform.Close();

		}

		private void Form1_FormClosed(object sender, FormClosedEventArgs e)
		{
			Console.WriteLine(Monitor.TryEnter(textbox));
		}

	}

	static class Extensions
	{
		public static List<T> Clone<T>(this IList<T> listToClone) where T : ICloneable
		{
			return listToClone.Select(item => (T)item.Clone()).ToList();
		}
	}
}
