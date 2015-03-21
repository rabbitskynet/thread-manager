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
	public partial class Form1 : Form
	{
		private Form2 taskform;
		private ThreadManager manager;
		public Form1()
		{
			InitializeComponent();
            this.Icon = Threader.Properties.Resources.threadicon;
			//for (char c = 'a'; c <= 'k'; c++)
			//{
			//	if(c != 'j' && c != 'i')
			//		this.tasklist.items.add(c);
			//}
			//for (enumOneThread ac = enumOneThread.A; ac <= enumOneThread.K; ac++)
			//{
			//	this.tasklist.Items.Add(ac);
			//}
			//for (enumStages s = enumStages.INIT; s <= enumStages.FINISH; s++)
			//{
			//	this.stages.Items.Add(s);
			//}
			taskform = new Form2();

			List<Stage> dict = new List<Stage>();

			dict.Add(new Stage(enumStages.INIT, new SubStage[]{
					new SubStage(enumSubStages.WORK,new OneThread[]{
						new OneThread(enumOneThread.A,()=>generate())
				})
			}));
			dict.Add(new Stage(enumStages.MAIN, new SubStage[]{
					new SubStage(enumSubStages.WORK,new OneThread[]{
						new OneThread(enumOneThread.B,()=>f1()),
						new OneThread(enumOneThread.C,()=>f2())
				}),
					new SubStage(enumSubStages.SUB1,new OneThread[]{
						new OneThread(enumOneThread.D,()=>f3())
				}),
					new SubStage(enumSubStages.SUB2,new OneThread[]{
						new OneThread(enumOneThread.E,()=>f4()),
						new OneThread(enumOneThread.F,()=>f5()),
						new OneThread(enumOneThread.G,()=>f6())
				}),
					new SubStage(enumSubStages.SUB3,new OneThread[]{
						new OneThread(enumOneThread.H,()=>f7())
				})
			}));
			dict.Add(new Stage(enumStages.FINISH, new SubStage[]{
					new SubStage(enumSubStages.WORK,new OneThread[]{
						new OneThread(enumOneThread.K,()=>f8())
				})
			}));

			manager = new ThreadManager(dict,this.textbox, this.statustreeview);
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
			this.manager.Start();
		}

		private void stopbutton_Click(object sender, EventArgs e)
		{

		}

		private void savebutton_Click(object sender, EventArgs e)
		{

		}

		public void generate()
		{
			Thread.Sleep(3000);
		}
		public void f1()
		{
			Thread.Sleep(9000);
		}
		public void f2()
		{
			Thread.Sleep(9000);
		}
		public void f3()
		{
			Thread.Sleep(3000);
		}
		public void f4()
		{
			Thread.Sleep(3000);
		}
		public void f5()
		{
			Thread.Sleep(3000);
		}
		public void f6()
		{
			Thread.Sleep(3000);
		}
		public void f7()
		{
			Thread.Sleep(3000);
		}
		public void f8()
		{
			Thread.Sleep(3000);
		}

	}
}
