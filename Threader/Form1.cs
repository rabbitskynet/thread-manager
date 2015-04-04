using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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
		private int sleeptime = 100;
		private int max = 10;
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
				new OneThread("A",(OneThread thr)=>generate(ref thr))
			}));
			dict.Add(new Stage("MAIN",new OneThread[]{
				new OneThread("B",(OneThread thr)=>f1(ref thr)),
				new OneThread("C",(OneThread thr)=>f2(ref thr))
			},
				new Stage[]{
					new Stage("SUB1",new OneThread[]{
						new OneThread("D",(OneThread thr)=>f3(ref thr))
				}),
					new Stage("SUB2",new OneThread[]{
						new OneThread("E",(OneThread thr)=>f4(ref thr)),
						new OneThread("F",(OneThread thr)=>f5(ref thr)),
						new OneThread("G",(OneThread thr)=>f6(ref thr))
				}),
					new Stage("SUB3",new OneThread[]{
						new OneThread("H",(OneThread thr)=>f7(ref thr))
				})
			}));
			dict.Add(new Stage("FINISH", new OneThread[]{
				new OneThread("K",(OneThread thr)=>f8(ref thr))
			}));

			manager = new ThreadManager(dict, this.textbox, this.statustreeview);
			manager.Completed += allstagescompleted;
			manager.ThreadCompleted += threadcompleted;
			manager.ThreadStarting += threadstarting;
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

		private void allstagescompleted(object sender, EventArgs e)
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

		private void threadcompleted(object sender, EventArgs e)
		{
			OneThread thr = (OneThread)sender;
			writeln("[" + thr.stopdate + "] Thread [" + thr.name + "] : Completed");
		}

		private void threadstarting(object sender, EventArgs e)
		{
			OneThread thr = (OneThread)sender;
			writeln("[" + thr.startdate + "] Thread [" + thr.name + "] : Created from [" + thr.getlastthread() + "]");
			writeln("[" + DateTime.Now + "] Thread [" + thr.name + "] : Started");
		}
		

		private void savebutton_Click(object sender, EventArgs e)
		{
			DialogResult result = this.saveFileDialog1.ShowDialog();
			if (result == System.Windows.Forms.DialogResult.OK )
			{
				this.textbox.SaveFile(this.saveFileDialog1.FileName, RichTextBoxStreamType.PlainText);
				MessageBox.Show("File saved to " + this.saveFileDialog1.FileName);
			}
		}

		public void write(string s)
		{
			try
			{
				this.textbox.Invoke((MethodInvoker)delegate()
				{
					this.textbox.AppendText(s);
				});
			}
			catch { }
		}
		public void writeln(string s)
		{
			try
			{
				this.textbox.Invoke((MethodInvoker)delegate()
				{
					this.textbox.AppendText(s + "\n");
				});
			}
			catch { };
		}

		private List<int[]> MX = new List<int[]>();
		private List<int[]> F1 = new List<int[]>();
		private List<int[]> F2 = new List<int[]>();
		private List<int[]> F3 = new List<int[]>();

		private int[] F4 = new int[0];
		private int[] F5 = new int[0];
		private int[] F6 = new int[0];

		private List<int[]> F7 = new List<int[]>();
		private List<int[]> F8 = new List<int[]>();


		public void generate(ref OneThread thr)
		{
			thr.progress = 0;
			Random r = new Random();
			int[] a = new int[this.max], b = new int[this.max], c = new int[this.max];
			for (int i = 0; i < this.max; i++)
			{
				a[i] = r.Next(10) * i;
				b[i] = r.Next(10) * i;
				c[i] = r.Next(10) * i;
				thr.progress += (int)((i+1)/this.max*0.9);
				Thread.Sleep(this.sleeptime);
			}
			lock (MX)
			{
				MX.Add(a);
				MX.Add(b);
				MX.Add(c);
				thr.progress += 10;
				Thread.Sleep(this.sleeptime);
			}

			string res = "[" + DateTime.Now + "] Thread [" + thr.name + "] : M1 [";
			for (int i = 0; i < this.max; i++)
				res += a[i] + (i < (this.max - 1) ? "," : "");
			writeln(res+"]");
			res="[" + DateTime.Now + "] Thread [" + thr.name + "] : M2 [";
			for (int i = 0; i < this.max; i++)
				res+=b[i] + (i < (this.max - 1) ? "," : "");
			writeln(res+"]");
			res="[" + DateTime.Now + "] Thread [" + thr.name + "] : M3 [";
			for (int i = 0; i < this.max; i++)
			{
				res+=c[i] + (i < (this.max - 1) ? "," : "");
			}
			writeln(res+"]");
			thr.progress = 100;
		}
		public void f1(ref OneThread thr)
		{
			List<int[]> arr;
			lock (MX)
			{
				arr = MX.Clone();
			}
			string res ="";
			int qe = 0;
			for (int q = 0; q < arr.Count; q++)
			{
				for (int i = 0; i < (this.max); i++)
				{
					qe++;
					arr[q][i] = arr[q][i] * 2;
					Console.WriteLine(qe*1.0 / (arr.Count * this.max) * 90);
					thr.progress = Convert.ToInt32(qe * 1.0 / (arr.Count * this.max) * 90);
					Thread.Sleep(this.sleeptime*5);
				}
			}

			lock(F1)
			{
				F1 = arr ;
			}

			for (int q = 0; q < arr.Count; q++)
			{
				res = "[" + DateTime.Now + "] Thread [" + thr.name + "] : M"+(q+1)+" [";
				for (int i = 0; i < (this.max); i++)
				{
					res+=arr[q][i] + (i < (this.max - 1) ? "," : "");
				}
				writeln(res+"]");
			}
			thr.progress = 100;
		}
		public void f2(ref OneThread thr)
		{
			List<int[]> arr;
			lock (MX)
			{
				arr = MX.Clone();
			}
			string res = "";
			int qe = 0;
			for (int q = 0; q < arr.Count; q++)
			{
				for (int i = 0; i < (this.max); i++)
				{
					qe++;
					arr[q][i] = arr[q][i] * 3;
					Console.WriteLine(qe * 1.0 / (arr.Count * this.max) * 90);
					thr.progress = Convert.ToInt32(qe * 1.0 / (arr.Count * this.max) * 90);
					Thread.Sleep(this.sleeptime * 2);
				}
			}

			lock (F2)
			{
				F2 = arr;
			}

			for (int q = 0; q < arr.Count; q++)
			{
				res = "[" + DateTime.Now + "] Thread [" + thr.name + "] : M" + (q+1) + " [";
				for (int i = 0; i < (this.max); i++)
				{
					res += arr[q][i] + (i < (this.max - 1) ? "," : "");
				}
				writeln(res + "]");
			}
			thr.progress = 100;
		}
		public void f3(ref OneThread thr)
		{
			List<int[]> arr;
			lock (MX)
			{
				arr = MX.Clone();
			}
			string res = "";
			int qe = 0;
			for (int q = 0; q < arr.Count; q++)
			{
				for (int i = 0; i < (this.max); i++)
				{
					qe++;
					arr[q][i] = arr[q][i] * 4;
					Console.WriteLine(qe * 1.0 / (arr.Count * this.max) * 90);
					thr.progress = Convert.ToInt32(qe * 1.0 / (arr.Count * this.max) * 90);
					Thread.Sleep(this.sleeptime);
				}
			}

			lock (F3)
			{
				F3 = arr;
			}

			for (int q = 0; q < arr.Count; q++)
			{
				res = "[" + DateTime.Now + "] Thread [" + thr.name + "] : M" + (q+1) + " [";
				for (int i = 0; i < (this.max); i++)
				{
					res += arr[q][i] + (i < (this.max - 1) ? "," : "");
				}
				writeln(res + "]");
			}
			thr.progress = 100;
		}
		public void f4(ref OneThread thr)
		{
			List<int[]> arr;
			lock (F3)
			{
				arr = F3.Clone();
			}
			string res = "";
			int qe = 0;
			int[] minarr = new int[this.max];
			for (int q = 0; q < arr.Count; q++)
			{
				for (int i = 0; i < (this.max); i++)
				{
					qe++;
					minarr[i] += arr[q][i];
					Console.WriteLine(qe * 1.0 / (arr.Count * this.max) * 90);
					thr.progress = Convert.ToInt32(qe * 1.0 / (arr.Count * this.max) * 90);
					Thread.Sleep(this.sleeptime * 2);
				}
			}

			lock (F4)
			{
				F4 = minarr;
			}

			res = "[" + DateTime.Now + "] Thread [" + thr.name + "] : AR [";
			for (int i = 0; i < (this.max); i++)
			{
				res += minarr[i] + (i < (this.max - 1) ? "," : "");
			}
			writeln(res + "]");
			thr.progress = 100;
		}
		public void f5(ref OneThread thr)
		{
			List<int[]> arr;
			lock (F3)
			{
				arr = F3.Clone();
			}
			string res = "";
			int qe = 0;
			int[] minarr = new int[this.max];
			for (int q = 0; q < arr.Count; q++)
			{
				for (int i = 0; i < (this.max); i++)
				{
					qe++;
					minarr[i] += arr[q][i] + (this.max - i) * 255;
					Console.WriteLine(qe * 1.0 / (arr.Count * this.max) * 90);
					thr.progress = Convert.ToInt32(qe * 1.0 / (arr.Count * this.max) * 90);
					Thread.Sleep(this.sleeptime * 2);
				}
			}

			lock (F5)
			{
				F5 = minarr;
			}

			res = "[" + DateTime.Now + "] Thread [" + thr.name + "] : AR [";
			for (int i = 0; i < (this.max); i++)
			{
				res += minarr[i] + (i < (this.max - 1) ? "," : "");
			}
			writeln(res + "]");
			thr.progress = 100;
		}
		public void f6(ref OneThread thr)
		{
			List<int[]> arr;
			lock (F3)
			{
				arr = F3.Clone();
			}
			string res = "";
			int qe = 0;
			int[] minarr = new int[this.max];
			for (int q = 0; q < arr.Count; q++)
			{
				for (int i = 0; i < (this.max); i++)
				{
					qe++;
					minarr[i] += arr[q][i]/2;
					Console.WriteLine(qe * 1.0 / (arr.Count * this.max) * 90);
					thr.progress = Convert.ToInt32(qe * 1.0 / (arr.Count * this.max) * 90);
					Thread.Sleep(this.sleeptime*2);
				}
			}

			lock (F6)
			{
				F6 = minarr;
			}

			res = "[" + DateTime.Now + "] Thread [" + thr.name + "] : AR [";
			for (int i = 0; i < (this.max); i++)
			{
				res += minarr[i] + (i < (this.max - 1) ? "," : "");
			}
			writeln(res + "]");
			thr.progress = 100;
		}
		public void f7(ref OneThread thr)
		{
			int[] arrF4;
			int[] arrF5;
			int[] arrF6;
			lock (F4)
			{
				arrF4 = F4;
			}
			lock (F5)
			{
				arrF5 = F5;
			}
			lock (F6)
			{
				arrF6 = F6;
			}
			string res = "";
			int qe = 0;
			List<int[]> minarr = new List<int[]>();
			for (int i = 0; i < (this.max); i++)
			{
				qe++;
				arrF4[i] += arrF4[i] / 2;
				thr.progress = Convert.ToInt32(qe * 1.0 / (this.max) * 30);
				Thread.Sleep(this.sleeptime );
			}
			minarr.Add(arrF4);
			for (int i = 0; i < (this.max); i++)
			{
				qe++;
				arrF5[i] += arrF5[i] / 2;
				thr.progress = Convert.ToInt32(qe * 1.0 / (this.max) * 30);
				Thread.Sleep(this.sleeptime);
			}
			minarr.Add(arrF5);
			for (int i = 0; i < (this.max); i++)
			{
				qe++;
				arrF6[i] += arrF6[i] / 2;
				thr.progress = Convert.ToInt32(qe * 1.0 / (this.max) * 30);
				Thread.Sleep(this.sleeptime);
			}
			minarr.Add(arrF6);
			lock (F7)
			{
				F7 = minarr;
			}

			for (int q = 0; q < minarr.Count; q++)
			{
				res = "[" + DateTime.Now + "] Thread [" + thr.name + "] : AR"+(q+1)+" [";
				for (int i = 0; i < (this.max); i++)
				{
					res += minarr[q][i] + (i < (this.max - 1) ? "," : "");
				}
				writeln(res + "]");
			}
			thr.progress = 100;
		}
		public void f8(ref OneThread thr)
		{
			List<int[]> arrF1;
			List<int[]> arrF2;
			List<int[]> arrF7;
			lock (F4)
			{
				arrF1 = F1;
			}
			lock (F5)
			{
				arrF2 = F2;
			}
			lock (F6)
			{
				arrF7 = F7;
			}
			string res = "";
			List<int[]> result = new List<int[]>();
			int qe = 0;
			int[] minarr = new int[this.max];
			for (int q = 0; q < arrF1.Count; q++)
			{
				for (int i = 0; i < (this.max); i++)
				{
					qe++;
					minarr[i] += arrF1[q][i];
					Console.WriteLine(qe * 1.0 / (arrF1.Count * this.max) * 30);
					thr.progress = Convert.ToInt32(qe * 1.0 / (arrF1.Count * this.max) * 30);
					Thread.Sleep(this.sleeptime * 2);
				}
			}
			result.Add(minarr);

			minarr = new int[this.max];
			for (int q = 0; q < arrF2.Count; q++)
			{
				for (int i = 0; i < (this.max); i++)
				{
					qe++;
					minarr[i] += arrF2[q][i];
					Console.WriteLine(qe * 1.0 / (arrF2.Count * this.max) * 30);
					thr.progress = Convert.ToInt32(qe * 1.0 / (arrF2.Count * this.max) * 30);
					Thread.Sleep(this.sleeptime * 2);
				}
			}
			result.Add(minarr);
			
			minarr = new int[this.max];
			for (int q = 0; q < arrF7.Count; q++)
			{
				for (int i = 0; i < (this.max); i++)
				{
					qe++;
					minarr[i] += arrF7[q][i];
					Console.WriteLine(qe * 1.0 / (arrF7.Count * this.max) * 30);
					thr.progress = Convert.ToInt32(qe * 1.0 / (arrF7.Count * this.max) * 30);
					Thread.Sleep(this.sleeptime * 2);
				}
			}
			result.Add(minarr);
			lock (F8)
			{
				F8 = result;
			}

			for (int q = 0; q < result.Count; q++)
			{
				res = "[" + DateTime.Now + "] Thread [" + thr.name + "] : AR" + (q + 1) + " [";
				for (int i = 0; i < (this.max); i++)
				{
					res += result[q][i] + (i < (this.max - 1) ? "," : "");
				}
				writeln(res + "]");
			}
			thr.progress = 100;
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

		private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
		{

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
