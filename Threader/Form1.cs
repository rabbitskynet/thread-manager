using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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
			//for (enumActions ac = enumActions.A; ac <= enumActions.K; ac++)
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
					new SubStage(enumSubStages.WORK,new Actions[]{
						new Actions(enumActions.A,(data)=>generate(data))
				})
			}));
			dict.Add(new Stage(enumStages.MAIN, new SubStage[]{
					new SubStage(enumSubStages.WORK,new Actions[]{
						new Actions(enumActions.B,(data)=>f1(data)),
						new Actions(enumActions.C,(data)=>f2(data))
				}),
					new SubStage(enumSubStages.SUB1,new Actions[]{
						new Actions(enumActions.D,(data)=>f3(data))
				}),
					new SubStage(enumSubStages.SUB2,new Actions[]{
						new Actions(enumActions.E,(data)=>f4(data)),
						new Actions(enumActions.F,(data)=>f5(data)),
						new Actions(enumActions.G,(data)=>f6(data))
				}),
					new SubStage(enumSubStages.SUB3,new Actions[]{
						new Actions(enumActions.H,(data)=>f7(data))
				})
			}));
			dict.Add(new Stage(enumStages.FINISH, new SubStage[]{
					new SubStage(enumSubStages.WORK,new Actions[]{
						new Actions(enumActions.K,(data)=>f8(data))
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

		public void generate(Object Data)
		{ }
		public void f1(Object Data)
		{ }
		public void f2(Object Data)
		{ }
		public void f3(Object Data)
		{ }
		public void f4(Object Data)
		{ }
		public void f5(Object Data)
		{ }
		public void f6(Object Data)
		{ }
		public void f7(Object Data)
		{ }
		public void f8(Object Data)
		{ }

	}
}
