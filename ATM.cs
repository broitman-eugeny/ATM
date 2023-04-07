//Òåñòîâîå çàäàíèå «Ðàçðàáîò÷èê C#»
//Çàäàíèå ¹2
//Ïðîãðàììà Áàíêîìàò

//Ïðîñòðàíñòâà èìåí
using System;//Çäåñü ñîäåðæèòñÿ íàáîð îñíîâíûõ êëàññîâ, è ìû çäåñü ñîçäàåì ñâîé êëàññ
using System.Windows.Forms;//Äëÿ ðàáîòû ñ ôîðìàìè
using System.Drawing;//Äîñòóï ê îñíîâíûì ôóíêöèÿì GDI
//using System.IO;//Ñèñòåìíûé ââîä âûâîä (ðàáîòà ñ ôàéëàìè)
//using System.Text;//Äëÿ Encoding
//using System.Windows.Controls;//Äëÿ TextChangedEventArgs

class MyForm : AppForm 
{
	public static MyForm appInstance;
		
	// Satisfies rule: MarkWindowsFormsEntryPointsWithStaThread.
	[STAThread]//Îäíîïîòî÷íîå ïðèëîæåíèå (äëÿ âîçìîæíîñòè èñïîëüçîâàíèÿ êîìïîíåíòîâ Windows Forms)
	public static void Main()
	{
		appInstance=new MyForm();
		Application.Run(appInstance);
	}
}

//Ñòðóêòóðà ñîñòîÿíèÿ áàíêîìàòà äëÿ îäíîãî äîñòîèíñòâà êóïþð
struct inATM
{
	public int amount;//Êîëè÷åñòâî êóïþð äàííîãî äîñòîèíñòâà, èìåþùèõñÿ â áàíêîìàòå
	public int maxAmount;//Ìàêñèìàëüíî âîçìîæíîå êîëè÷åñòâî êóïþð äàííîãî äîñòîèíñòâà, ïîìåùàåìîå â áàíêîìàò
	public Label lbl;//Ìåòêà êîëè÷åñòâà êóïþð, èìåþùèõñÿ â áàíêîìàòå	
}

//Ôîðìà ïðèëîæåíèÿ
class AppForm : Form
{
	//Èíäåêñû ýëåìåíòîâ â ìàññèâå stateOfATM â ñîîòâåòñòâèè ñ äîñòîèíñòâîì êóïþð
	const int d10=0;
	const int d50=1;
	const int d100=2;
	const int d500=3;
	const int d1000=4;
	const int d5000=5;
	//Ìàññèâ ñîñòîÿíèÿ áàíêîìàòà (êîëè÷åñòâî è äîñòîèíñòâî êóïþð)
	private inATM[] stateOfATM = new inATM[6];
	
	//Ýêçåìïëÿð êëàññà ãëàâíîé ôîðìû ïîëó÷åíèÿ íàëè÷íûõ
	public static GetCashForm gcForm;
	
	//Ôóíêöèÿ ïîëó÷åíèÿ ñîñòîÿíèÿ áàíêîìàòà
	public inATM[] getStateOfATM()
	{
		return this.stateOfATM;
	}
	
	//Ôóíêöèÿ óñòàíîâêè êîëè÷åñòâà êóïþð çàäàííîãî äîñòîèíñòâà â áàíêîìàòå è ñîîòâåòñòâóþùåé ìåòêè ñîñòîÿíèÿ áàíêîìàòà
	//digIndex - èíäåêñ äîñòîèíñòâà êóïþð
	//amount - êîëè÷åñòâî êóïþð äàííîãî äîñòîèíñòâà
	public void setBillsInATM(int digIndex, int amount)
	{
		string s="";
		switch(digIndex)
		{
			case d10: s="10";
				break;
			case d50: s="50";
				break;
			case d100: s="100";
				break;
			case d500: s="500";
				break;
			case d1000: s="1000";
				break;
			case d5000: s="5000";
				break;
			default: break;
		}
		if(amount>this.stateOfATM[digIndex].maxAmount)
		{
			MessageBox.Show("×èñëî êóïþð ïðåâûøàåò ìàêñèìàëüíî äîïóñòèìîå","Êóïþðû "+s+" ðóá.");
			return;
		}
		this.stateOfATM[digIndex].amount=amount;		
		this.stateOfATM[digIndex].lbl.Text=s+" ðóá.: "+amount.ToString()+" øò. èç ìàêñ.="+this.stateOfATM[digIndex].maxAmount.ToString()+" øò.";
	}

	//Ôóíêöèÿ óñòàíîâêè ìàêñèìàëüíî âîçìîæíîãî êîëè÷åñòâà êóïþð äàííîãî äîñòîèíñòâà, ïîìåùàåìîãî â áàíêîìàò
	//digIndex - èíäåêñ äîñòîèíñòâà êóïþð â ñîîòâåòñòâèè ñ enum digIndexes
	//maxAmount - ìàêñèìàëüíî âîçìîæíîå êîëè÷åñòâî êóïþð äàííîãî äîñòîèíñòâà
	public void setMaxBillsInATM(int digIndex, int maxAmount)
	{
		this.stateOfATM[digIndex].maxAmount=maxAmount;		
	}
	
	public AppForm()
	{
		this.Text="Áàíêîìàò";//Çàãîëîâîê îêíà ïðîãðàììû
		this.Width=250;//Øèðèíà îêíà
		this.Height=250;//Âûñîòà îêíà

		//Ìåíþ ïîãðàììû
		MainMenu mnuFileMenu = new MainMenu();
		this.Menu = mnuFileMenu;
		MenuItem MenuItemFile = new MenuItem("&File");
		MenuItemFile.MenuItems.Add("E&xit", new System.EventHandler(this.MenuExit_Click));
		MenuItem MenuItemOperations = new MenuItem("&Operations");
		MenuItemOperations.MenuItems.Add("&Contribute to ATM",new System.EventHandler(this.MenuContribute_Click));
		MenuItemOperations.MenuItems.Add("&Get cash from ATM",new System.EventHandler(this.MenuGet_Click));
		mnuFileMenu.MenuItems.Add(MenuItemFile);
		mnuFileMenu.MenuItems.Add(MenuItemOperations);

		//Èíèöèàëèçàöèÿ ìåòîê ñîñòîÿíèÿ áàíêîìàòà (êîëè÷åñòâî èìåþùèõñÿ êóïþð, ìàêñèìàëüíîå êîëè÷åñòâî)
		//Îáùàÿ íàäïèñü
		Label labelInATM = new Label();
		labelInATM.Text = "Íàëè÷èå êóïþð â áàíêîìàòå:";
		labelInATM.Location = new Point(15,15);
		labelInATM.AutoSize=true;
		labelInATM.TextAlign=ContentAlignment.BottomLeft;
		this.Controls.Add(labelInATM);
		int vShift=10;//Äîïîëíèòåëüíûé âåðòèêàëüíûé ñäâèã ìåæäó ìåòêàìè
		//Ìåòêè ñîñòîÿíèÿ áàíêîìàòà ïî äîñòîèíñòâàì êóïþð
		for(int i=d10; i<=d5000; i++)
		{
			setMaxBillsInATM(i, 1000);
			this.stateOfATM[i].lbl=new Label();
			setBillsInATM(i, 0);
			if(i>d10)//Âåðõíÿÿ ìåòêà ïî äîñòîèíñòâó êóïþð
			{
				vShift+=this.stateOfATM[i-1].lbl.Height+2;
			}
			this.stateOfATM[i].lbl.Location = new Point(15,labelInATM.Location.Y+labelInATM.Height+vShift);
			this.stateOfATM[i].lbl.AutoSize=true;
			this.stateOfATM[i].lbl.TextAlign=ContentAlignment.BottomLeft;
			this.Controls.Add(this.stateOfATM[i].lbl);
		}	
	}
	
	//Ïðè âûáîðå ìåíþ File\Exit
	private void MenuExit_Click(Object sender, EventArgs e)
	{
   		Application.Exit();
	}
	
	//Ïðè âûáîðå ìåíþ Operations\Contribute to ATM
	public void MenuContribute_Click(Object sender, EventArgs e)
	{
		ContribForm cForm=new ContribForm();
		cForm.ShowDialog(this);
		cForm.Dispose();
	}
	
	//Ïðè âûáîðå ìåíþ Operations\Get cash from ATM
	private void MenuGet_Click(Object sender, EventArgs e)
	{
		gcForm=new GetCashForm();
		gcForm.ShowDialog(this);
		gcForm.Dispose();
	}
}



//Ôîðìà âíåñåíèÿ ñðåäñòâ
class ContribForm : Form
{
	//Èíäåêñû ýëåìåíòîâ â ìàññèâå stateOfATM â ñîîòâåòñòâèè ñ äîñòîèíñòâîì êóïþð
	const int d10=0;
	const int d50=1;
	const int d100=2;
	const int d500=3;
	const int d1000=4;
	const int d5000=5;
	
	//Ýëåìåíòû ââîäà êîëè÷åñòâà êóïþð äëÿ âíåñåíèÿ â áàíêîìàò
	private myTextBox tbContrib10Rub;//10 ðóáëåâêè
	private myTextBox tbContrib50Rub;//50 ðóáëåâêè
	private myTextBox tbContrib100Rub;//100 ðóáëåâêè
	private myTextBox tbContrib500Rub;//500 ðóáëåâêè
	private myTextBox tbContrib1000Rub;//1000 ðóáëåâêè
	private myTextBox tbContrib5000Rub;//5000 ðóáëåâêè
	
	public ContribForm()
	{
		Text="Âíåñåíèå íàëè÷íûõ ñðåäñòâ â áàíêîìàò";//Çàãîëîâîê îêíà âíåñåíèÿ ñðåäñòâ

		//Ìåòêà - ïðèãëàøåíèå
		Label labelInvitation = new Label();
		labelInvitation.Text = "Óêàæèòå êîëè÷åñòâî âíîñèìûõ êóïþð äîñòîèíñòâîì:";
		labelInvitation.Location = new Point(15,15);
		labelInvitation.AutoSize=true;
		labelInvitation.TextAlign=ContentAlignment.BottomLeft;
		this.Controls.Add(labelInvitation);

		//Ìåòêà äëÿ ïîëÿ âíåñåíèÿ 10 ðóáëåâûõ êóïþð
		Label labelContrib10Rub = new Label();
		labelContrib10Rub.Text = "10 ðóá.:";
		labelContrib10Rub.Location = new Point(15,labelInvitation.Height+25);
		labelContrib10Rub.AutoSize=true;
		labelContrib10Rub.TextAlign=ContentAlignment.BottomLeft;
		this.Controls.Add(labelContrib10Rub);
		
		//Textbox âíåñåíèÿ 10 ðóáëåâûõ êóïþð
		tbContrib10Rub=new myTextBox();
		tbContrib10Rub.Text = "";
		tbContrib10Rub.Location=new Point(labelContrib10Rub.Width+17,labelInvitation.Height+25);
		tbContrib10Rub.Width=30;
		tbContrib10Rub.Height=labelContrib10Rub.Height;
		tbContrib10Rub.TextAlign=HorizontalAlignment.Right;
		this.Controls.Add(tbContrib10Rub);

		//Ìåòêà äëÿ ïîëÿ âíåñåíèÿ 50 ðóáëåâûõ êóïþð
		Label labelContrib50Rub = new Label();
		labelContrib50Rub.Text = "50 ðóá.:";
		labelContrib50Rub.Location = new Point(15,tbContrib10Rub.Location.Y+tbContrib10Rub.Height+2);
		labelContrib50Rub.AutoSize=true;
		labelContrib50Rub.TextAlign=ContentAlignment.BottomLeft;
		this.Controls.Add(labelContrib50Rub);
		
		//Textbox âíåñåíèÿ 50 ðóáëåâûõ êóïþð
		tbContrib50Rub=new myTextBox();
		tbContrib50Rub.Text = "";
		tbContrib50Rub.Location=new Point(labelContrib50Rub.Width+17,tbContrib10Rub.Location.Y+tbContrib10Rub.Height+2);
		tbContrib50Rub.Width=30;
		tbContrib50Rub.Height=labelContrib50Rub.Height;
		tbContrib50Rub.TextAlign=HorizontalAlignment.Right;
		this.Controls.Add(tbContrib50Rub);
		
		//Ìåòêà äëÿ ïîëÿ âíåñåíèÿ 100 ðóáëåâûõ êóïþð
		Label labelContrib100Rub = new Label();
		labelContrib100Rub.Text = "100 ðóá.:";
		labelContrib100Rub.Location = new Point(15,tbContrib50Rub.Location.Y+tbContrib50Rub.Height+2);
		labelContrib100Rub.AutoSize=true;
		labelContrib100Rub.TextAlign=ContentAlignment.BottomLeft;
		this.Controls.Add(labelContrib100Rub);
		
		//Textbox âíåñåíèÿ 100 ðóáëåâûõ êóïþð
		tbContrib100Rub=new myTextBox();
		tbContrib100Rub.Text = "";
		tbContrib100Rub.Location=new Point(labelContrib100Rub.Width+17,tbContrib50Rub.Location.Y+tbContrib50Rub.Height+2);
		tbContrib100Rub.Width=30;
		tbContrib100Rub.Height=labelContrib100Rub.Height;
		tbContrib100Rub.TextAlign=HorizontalAlignment.Right;
		this.Controls.Add(tbContrib100Rub);
		
		//Ìåòêà äëÿ ïîëÿ âíåñåíèÿ 500 ðóáëåâûõ êóïþð
		Label labelContrib500Rub = new Label();
		labelContrib500Rub.Text = "500 ðóá.:";
		labelContrib500Rub.Location = new Point(15,tbContrib100Rub.Location.Y+tbContrib100Rub.Height+2);
		labelContrib500Rub.AutoSize=true;
		labelContrib500Rub.TextAlign=ContentAlignment.BottomLeft;
		this.Controls.Add(labelContrib500Rub);
		
		//Textbox âíåñåíèÿ 500 ðóáëåâûõ êóïþð
		tbContrib500Rub=new myTextBox();
		tbContrib500Rub.Text = "";
		tbContrib500Rub.Location=new Point(labelContrib500Rub.Width+17,tbContrib100Rub.Location.Y+tbContrib100Rub.Height+2);
		tbContrib500Rub.Width=30;
		tbContrib500Rub.Height=labelContrib500Rub.Height;
		tbContrib500Rub.TextAlign=HorizontalAlignment.Right;
		this.Controls.Add(tbContrib500Rub);
		
		//Ìåòêà äëÿ ïîëÿ âíåñåíèÿ 1000 ðóáëåâûõ êóïþð
		Label labelContrib1000Rub = new Label();
		labelContrib1000Rub.Text = "1000 ðóá.:";
		labelContrib1000Rub.Location = new Point(15,tbContrib500Rub.Location.Y+tbContrib500Rub.Height+2);
		labelContrib1000Rub.AutoSize=true;
		labelContrib1000Rub.TextAlign=ContentAlignment.BottomLeft;
		this.Controls.Add(labelContrib1000Rub);
		
		//Textbox âíåñåíèÿ 1000 ðóáëåâûõ êóïþð
		tbContrib1000Rub=new myTextBox();
		tbContrib1000Rub.Text = "";
		tbContrib1000Rub.Location=new Point(labelContrib1000Rub.Width+17,tbContrib500Rub.Location.Y+tbContrib500Rub.Height+2);
		tbContrib1000Rub.Width=30;
		tbContrib1000Rub.Height=labelContrib1000Rub.Height;
		tbContrib1000Rub.TextAlign=HorizontalAlignment.Right;
		this.Controls.Add(tbContrib1000Rub);
		
		//Ìåòêà äëÿ ïîëÿ âíåñåíèÿ 5000 ðóáëåâûõ êóïþð
		Label labelContrib5000Rub = new Label();
		labelContrib5000Rub.Text = "5000 ðóá.:";
		labelContrib5000Rub.Location = new Point(15,tbContrib1000Rub.Location.Y+tbContrib1000Rub.Height+2);
		labelContrib5000Rub.AutoSize=true;
		labelContrib5000Rub.TextAlign=ContentAlignment.BottomLeft;
		this.Controls.Add(labelContrib5000Rub);
		
		//Textbox âíåñåíèÿ 5000 ðóáëåâûõ êóïþð
		tbContrib5000Rub=new myTextBox();
		tbContrib5000Rub.Text = "";
		tbContrib5000Rub.Location=new Point(labelContrib5000Rub.Width+17,tbContrib1000Rub.Location.Y+tbContrib1000Rub.Height+2);
		tbContrib5000Rub.Width=30;
		tbContrib5000Rub.Height=labelContrib5000Rub.Height;
		tbContrib5000Rub.TextAlign=HorizontalAlignment.Right;
		this.Controls.Add(tbContrib5000Rub);
		
		//Êíîïêà âíåñåíèÿ ñðåäñòâ
		Button bContrib=new Button();
		bContrib.Text="Âíåñòè";
		bContrib.Location=new Point(15,tbContrib5000Rub.Location.Y+tbContrib1000Rub.Height+10);
		bContrib.AutoSize=true;
		bContrib.TextAlign=ContentAlignment.MiddleCenter;
		this.Controls.Add(bContrib);
		bContrib.Click += new System.EventHandler(bContrib_Clicked);//Óñòàíîâêà îáðàáîò÷èêà íàæàòèÿ íà êíîïêó âíåñåíèÿ ñðåäñòâ
	}

	//Ïîëó÷èòü ÷èñëåííîå çíà÷åíèå myTextBox
	public int getIntValMTB(myTextBox mTB)
	{
		return (mTB.Text=="")?0:Convert.ToInt32(mTB.Text);
	}
	
	//Îáðàáîò÷èê íàæàòèÿ íà êíîïêó âíåñåíèÿ ñðåäñòâ
	protected void bContrib_Clicked(object sender, EventArgs e)
	{
		//Êîëè÷åñòâî êóïþð â áàíêîìàòå äî ïîïîëíåíèÿ
		int billsBeforeContrib10=MyForm.appInstance.getStateOfATM()[d10].amount;
		int billsBeforeContrib50=MyForm.appInstance.getStateOfATM()[d50].amount;
		int billsBeforeContrib100=MyForm.appInstance.getStateOfATM()[d100].amount;
		int billsBeforeContrib500=MyForm.appInstance.getStateOfATM()[d500].amount;
		int billsBeforeContrib1000=MyForm.appInstance.getStateOfATM()[d1000].amount;
		int billsBeforeContrib5000=MyForm.appInstance.getStateOfATM()[d5000].amount;		
		//Ïðåäïîëàãàåìîå êîëè÷åñòâî êóïþð â áàíêîìàòå ïîñëå ïîïîëíåíèÿ
		int billsAfterContrib10=billsBeforeContrib10+getIntValMTB(tbContrib10Rub);
		int billsAfterContrib50=billsBeforeContrib50+getIntValMTB(tbContrib50Rub);
		int billsAfterContrib100=billsBeforeContrib100+getIntValMTB(tbContrib100Rub);
		int billsAfterContrib500=billsBeforeContrib500+getIntValMTB(tbContrib500Rub);
		int billsAfterContrib1000=billsBeforeContrib1000+getIntValMTB(tbContrib1000Rub);
		int billsAfterContrib5000=billsBeforeContrib5000+getIntValMTB(tbContrib5000Rub);
		
		//Âíåñåíèå 10 ðóáëåâûõ êóïþð
		MyForm.appInstance.setBillsInATM(d10, billsAfterContrib10);
		//Âíåñåíèå 50 ðóáëåâûõ êóïþð
		MyForm.appInstance.setBillsInATM(d50, billsAfterContrib50);
		//Âíåñåíèå 100 ðóáëåâûõ êóïþð
		MyForm.appInstance.setBillsInATM(d100, billsAfterContrib100);
		//Âíåñåíèå 500 ðóáëåâûõ êóïþð
		MyForm.appInstance.setBillsInATM(d500, billsAfterContrib500);
		//Âíåñåíèå 1000 ðóáëåâûõ êóïþð
		MyForm.appInstance.setBillsInATM(d1000, billsAfterContrib1000);
		//Âíåñåíèå 5000 ðóáëåâûõ êóïþð
		MyForm.appInstance.setBillsInATM(d5000, billsAfterContrib5000);
		if(billsAfterContrib10>MyForm.appInstance.getStateOfATM()[d10].maxAmount ||//Êîëè÷åñòâî êàêèõ ëèáî èç êóïþð ïðåâûøàåò ìàêñèìàëüíî äîïóñòèìîå â áàíêîìàòå
			billsAfterContrib50>MyForm.appInstance.getStateOfATM()[d50].maxAmount ||
			billsAfterContrib100>MyForm.appInstance.getStateOfATM()[d100].maxAmount ||
			billsAfterContrib500>MyForm.appInstance.getStateOfATM()[d500].maxAmount ||
			billsAfterContrib1000>MyForm.appInstance.getStateOfATM()[d1000].maxAmount ||
			billsAfterContrib5000>MyForm.appInstance.getStateOfATM()[d5000].maxAmount)
		{
			if(!(MyForm.appInstance.getStateOfATM()[d10].amount==billsBeforeContrib10 &&//Êàêèå ëèáî èç êóïþð âíåñåíû â áàíêîìàò
			MyForm.appInstance.getStateOfATM()[d50].amount==billsBeforeContrib50 &&
			MyForm.appInstance.getStateOfATM()[d100].amount==billsBeforeContrib100 &&
			MyForm.appInstance.getStateOfATM()[d500].amount==billsBeforeContrib500 &&
			MyForm.appInstance.getStateOfATM()[d1000].amount==billsBeforeContrib1000 &&
			MyForm.appInstance.getStateOfATM()[d5000].amount==billsBeforeContrib5000))
			{
				MessageBox.Show("Îñòàëüíûå êóïþðû âíåñåíû óñïåøíî", "Âíåñåíèå ñðåäñòâ");
			}			
		}
		else
		{
			if(!(tbContrib10Rub.Text=="" &&//Êàêèå ëèáî èç êóïþð âíåñåíû â áàíêîìàò
			tbContrib50Rub.Text=="" &&
			tbContrib100Rub.Text=="" &&
			tbContrib500Rub.Text=="" &&
			tbContrib1000Rub.Text=="" &&
			tbContrib5000Rub.Text==""))
			{
				MessageBox.Show("Êóïþðû âíåñåíû óñïåøíî", "Âíåñåíèå ñðåäñòâ");
				this.Hide();
			}
		}
		tbContrib10Rub.Text="";
		tbContrib50Rub.Text="";
		tbContrib100Rub.Text="";
		tbContrib500Rub.Text="";
		tbContrib1000Rub.Text="";
		tbContrib5000Rub.Text="";
	}
}

//Ãëàâíàÿ ôîðìà ïîëó÷åíèÿ íàëè÷íûõ
class GetCashForm : Form
{
	//Èíäåêñû ýëåìåíòîâ â ìàññèâå stateOfATM â ñîîòâåòñòâèè ñ äîñòîèíñòâîì êóïþð
	const int d10=0;
	const int d50=1;
	const int d100=2;
	const int d500=3;
	const int d1000=4;
	const int d5000=5;
	
	//Ýëåìåíò ââîäà çàïðàøèâàåìîé ñóììû
	public myTextBox tbGetCash;
	//Ýëåìåíòû âûáîðà êðóïíûõ/ìåëêèõ êóïþð
	private RadioButton rbLargeBills;//êðóïíûå êóïþðû
	private RadioButton rbSmallBills;//ìåëêèå êóïþðû
	
	public GetCashForm()
	{
		Text="Ïîëó÷åíèå íàëè÷íûõ ñðåäñòâ";//Çàãîëîâîê îêíà ïîëó÷åíèÿ ñðåäñòâ
		
		//Ìåòêà - ïðèãëàøåíèå
		Label labelInvitation = new Label();
		labelInvitation.Text = "Óêàæèòå òðåáóåìóþ ñóììó â ðóáëÿõ:";
		labelInvitation.Location = new Point(15,15);
		labelInvitation.AutoSize=true;
		labelInvitation.TextAlign=ContentAlignment.BottomLeft;
		this.Controls.Add(labelInvitation);
		
		//Textbox äëÿ ââîäà çàïðàøèâàåìîé ñóììû
		tbGetCash=new myTextBox();
		tbGetCash.Text = "";
		tbGetCash.Location=new Point(labelInvitation.Width+17,15);
		tbGetCash.Width=50;
		tbGetCash.Height=labelInvitation.Height;
		tbGetCash.TextAlign=HorizontalAlignment.Right;
		this.Controls.Add(tbGetCash);
		
		//GroupBox äëÿ âûáîðà êðóïíûõ/ìåëêèõ êóïþð
		GroupBox gbLargeSmallBills = new GroupBox();		
		rbLargeBills = new RadioButton();
		rbSmallBills = new RadioButton();
		gbLargeSmallBills.Controls.Add(rbLargeBills);
		gbLargeSmallBills.Controls.Add(rbSmallBills);
		gbLargeSmallBills.Location = new Point(15, tbGetCash.Height+17);
		gbLargeSmallBills.Width=rbLargeBills.Width+rbSmallBills.Width+32;
		gbLargeSmallBills.Height=rbLargeBills.Height+30;
		gbLargeSmallBills.Text = "Êàêèìè êóïþðàìè âûäàòü ñóììó?";
		rbLargeBills.Text = "Êðóïíûìè";
		rbSmallBills.Text = "Ñ ðàçìåíîì";
		rbLargeBills.Location = new Point(15, 20);
		rbSmallBills.Location = new Point(rbLargeBills.Width+17, 20);
		rbLargeBills.AutoSize=true;
		rbSmallBills.AutoSize=true;
		Controls.Add(gbLargeSmallBills);
		rbLargeBills.Checked=true;
		
		//Êíîïêà ïîëó÷åíèÿ íàëè÷íûõ
		Button bGet=new Button();
		bGet.Text="Ïîëó÷èòü";
		bGet.Location=new Point(15,gbLargeSmallBills.Height+tbGetCash.Height+19);
		bGet.AutoSize=true;
		bGet.TextAlign=ContentAlignment.MiddleCenter;
		this.Controls.Add(bGet);
		bGet.Click += new System.EventHandler(bGet_Clicked);//Óñòàíîâêà îáðàáîò÷èêà íàæàòèÿ íà êíîïêó ïîëó÷åíèÿ íàëè÷íûõ
	}
	
	//Ïîëó÷èòü ÷èñëåííîå çíà÷åíèå myTextBox
	public int getIntValMTB(myTextBox mTB)
	{
		return (mTB.Text=="")?0:Convert.ToInt32(mTB.Text);
	}
	
	//Îáðàáîò÷èê íàæàòèÿ íà êíîïêó ïîëó÷åíèÿ íàëè÷íûõ
	protected void bGet_Clicked(object sender, EventArgs e)
	{
		//Êîëè÷åñòâî êóïþð âûäàâàåìûõ áàíêîìàòîì
		int numBills5000;
		int numBills1000;
		int numBills500;
		int numBills100;
		int numBills50;
		int numBills10;
		//Êîëè÷åñòâî êóïþð â áàíêîìàòå äî ïîëó÷åíèÿ íàëè÷íûõ
		int billsBeforeGet10=MyForm.appInstance.getStateOfATM()[d10].amount;
		int billsBeforeGet50=MyForm.appInstance.getStateOfATM()[d50].amount;
		int billsBeforeGet100=MyForm.appInstance.getStateOfATM()[d100].amount;
		int billsBeforeGet500=MyForm.appInstance.getStateOfATM()[d500].amount;
		int billsBeforeGet1000=MyForm.appInstance.getStateOfATM()[d1000].amount;
		int billsBeforeGet5000=MyForm.appInstance.getStateOfATM()[d5000].amount;
		//Òðåáóåìàÿ ñóììà
		int sumReq=getIntValMTB(tbGetCash);		
		//Âû÷èñëåíèå êîëè÷åñòâà êóïþð äëÿ âûäà÷è òðåáóåìîé ñóììû
		if(sumReq>0)//Òðåáóåìàÿ ñóììà íå íóëåâàÿ
		{
			if(rbLargeBills.Checked==true)//Âûáðàíî êðóïíûìè êóïþðàìè
			{
				numBills5000=sumReq/5000;//Êîëè÷åñòâî 5000 êóïþð, òðåáóåìûõ äëÿ çàïðàøèâàåìîé ñóììû
				if(billsBeforeGet5000<numBills5000)//Åñëè â áàíêîìàòå íåò ñòîëüêî 5000 êóïþð
				{
					numBills5000=billsBeforeGet5000;//Çàáèðàåì âñå èç áàíêîìàòà
				}
				int remains=sumReq-(5000*numBills5000);//Îñòàòîê çàïðàøèâàåìîé ñóììû ïîñëå èñêëþ÷åíèÿ ñóììû îáåñïå÷èâàåìîé 5000 êóïþðàìè
				numBills1000=remains/1000;//Êîëè÷åñòâî 1000 êóïþð, òðåáóåìûõ äëÿ îñòàòêà çàïðàøèâàåìîé ñóììû
				if(billsBeforeGet1000<numBills1000)//Åñëè â áàíêîìàòå íåò ñòîëüêî 1000 êóïþð
				{
					numBills1000=billsBeforeGet1000;//Çàáèðàåì âñå èç áàíêîìàòà
				}
				remains-=1000*numBills1000;//Îñòàòîê çàïðàøèâàåìîé ñóììû ïîñëå èñêëþ÷åíèÿ ñóììû îáåñïå÷èâàåìîé 1000 êóïþðàìè
				numBills500=remains/500;//Êîëè÷åñòâî 500 êóïþð, òðåáóåìûõ äëÿ îñòàòêà çàïðàøèâàåìîé ñóììû
				if(billsBeforeGet500<numBills500)//Åñëè â áàíêîìàòå íåò ñòîëüêî 500 êóïþð
				{
					numBills500=billsBeforeGet500;//Çàáèðàåì âñå èç áàíêîìàòà
				}
				remains-=500*numBills500;//Îñòàòîê çàïðàøèâàåìîé ñóììû ïîñëå èñêëþ÷åíèÿ ñóììû îáåñïå÷èâàåìîé 500 êóïþðàìè
				numBills100=remains/100;//Êîëè÷åñòâî 100 êóïþð, òðåáóåìûõ äëÿ îñòàòêà çàïðàøèâàåìîé ñóììû
				if(billsBeforeGet100<numBills100)//Åñëè â áàíêîìàòå íåò ñòîëüêî 100 êóïþð
				{
					numBills100=billsBeforeGet100;//Çàáèðàåì âñå èç áàíêîìàòà
				}
				remains-=100*numBills100;//Îñòàòîê çàïðàøèâàåìîé ñóììû ïîñëå èñêëþ÷åíèÿ ñóììû îáåñïå÷èâàåìîé 100 êóïþðàìè
				numBills50=remains/50;//Êîëè÷åñòâî 50 êóïþð, òðåáóåìûõ äëÿ îñòàòêà çàïðàøèâàåìîé ñóììû
				if(billsBeforeGet50<numBills50)//Åñëè â áàíêîìàòå íåò ñòîëüêî 50 êóïþð
				{
					numBills50=billsBeforeGet50;//Çàáèðàåì âñå èç áàíêîìàòà
				}
				remains-=50*numBills50;//Îñòàòîê çàïðàøèâàåìîé ñóììû ïîñëå èñêëþ÷åíèÿ ñóììû îáåñïå÷èâàåìîé 50 êóïþðàìè
				numBills10=remains/10;//Êîëè÷åñòâî 10 êóïþð, òðåáóåìûõ äëÿ îñòàòêà çàïðàøèâàåìîé ñóììû
				if(billsBeforeGet10<numBills10)//Åñëè â áàíêîìàòå íåò ñòîëüêî 10 êóïþð
				{
					MessageBox.Show("Â áàíêîìàòå îòñóòñòâóåò òðåáóåìàÿ ñóììà ëèáî êóïþðû òðåáóåìîãî äîñòîèíñòâà.\nÂâåäèòå äðóãîå çíà÷åíèå", "Ïîëó÷åíèå íàëè÷íûõ ñðåäñòâ");
					return;
				}
				remains-=10*numBills10;//Îñòàòîê çàïðàøèâàåìîé ñóììû ïîñëå èñêëþ÷åíèÿ ñóììû îáåñïå÷èâàåìîé 10 êóïþðàìè
				if(remains>0)//Ñóììà íå êðàòíà 10
				{
					MessageBox.Show("Îòñóòñòâóþò êóïþðû äîñòîèíñòâîì ìåíüøå 10 ðóá.\nÂâåäèòå çíà÷åíèå êðàòíîå 10", "Ïîëó÷åíèå íàëè÷íûõ ñðåäñòâ");
					return;
				}
				//Èçúÿòèå 10 ðóáëåâûõ êóïþð èç áàíêîìàòà
				MyForm.appInstance.setBillsInATM(d10, billsBeforeGet10-numBills10);
				//Èçúÿòèå 50 ðóáëåâûõ êóïþð èç áàíêîìàòà
				MyForm.appInstance.setBillsInATM(d50, billsBeforeGet50-numBills50);
				//Èçúÿòèå 100 ðóáëåâûõ êóïþð èç áàíêîìàòà
				MyForm.appInstance.setBillsInATM(d100, billsBeforeGet100-numBills100);
				//Èçúÿòèå 500 ðóáëåâûõ êóïþð èç áàíêîìàòà
				MyForm.appInstance.setBillsInATM(d500, billsBeforeGet500-numBills500);
				//Èçúÿòèå 1000 ðóáëåâûõ êóïþð èç áàíêîìàòà
				MyForm.appInstance.setBillsInATM(d1000, billsBeforeGet1000-numBills1000);
				//Èçúÿòèå 5000 ðóáëåâûõ êóïþð èç áàíêîìàòà
				MyForm.appInstance.setBillsInATM(d5000, billsBeforeGet5000-numBills5000);
		
				//Ñòðîêîâûå âñòàâêè â ñîîáùåíèå êëèåíòó î êîëè÷åñòâå è íîìèíàëå ïîëó÷àåìûõ êóïþð
				string strNumGet10Rub=(numBills10>0)?("\n"+numBills10.ToString()+" - 10 ðóáëåâûõ êóïþð"):"";
				string strNumGet50Rub=(numBills50>0)?("\n"+numBills50.ToString()+" - 50 ðóáëåâûõ êóïþð"):"";
				string strNumGet100Rub=(numBills100>0)?("\n"+numBills100.ToString()+" - 100 ðóáëåâûõ êóïþð"):"";
				string strNumGet500Rub=(numBills500>0)?("\n"+numBills500.ToString()+" - 500 ðóáëåâûõ êóïþð"):"";
				string strNumGet1000Rub=(numBills1000>0)?("\n"+numBills1000.ToString()+" - 1000 ðóáëåâûõ êóïþð"):"";
				string strNumGet5000Rub=(numBills5000>0)?("\n"+numBills5000.ToString()+" - 5000 ðóáëåâûõ êóïþð"):"";
		
				MessageBox.Show("Ïîëó÷èòå "+(numBills10*10+numBills50*50+numBills100*100+numBills500*500+numBills1000*1000+numBills5000*5000).ToString()+" ðóá.:"+strNumGet10Rub+strNumGet50Rub+strNumGet100Rub+strNumGet500Rub+strNumGet1000Rub+strNumGet5000Rub,
				"Ïîëó÷åíèå íàëè÷íûõ ñðåäñòâ êðóïíûìè êóïþðàìè");				
			}
			else//Âûáðàíî êóïþðàìè c ðàçìåíîì
			{
				GetCashExchangeForm gcEForm=new GetCashExchangeForm();
				gcEForm.ShowDialog(this);
				gcEForm.Dispose();
			}
			this.Hide();
		}
	}
}

//Ôîðìà ïîëó÷åíèÿ íàëè÷íûõ ñ ðàçìåíîì
class GetCashExchangeForm : Form
{
	//Èíäåêñû ýëåìåíòîâ â ìàññèâå stateOfATM â ñîîòâåòñòâèè ñ äîñòîèíñòâîì êóïþð
	const int d10=0;
	const int d50=1;
	const int d100=2;
	const int d500=3;
	const int d1000=4;
	const int d5000=5;
	
	//Ýëåìåíòû ââîäà êîëè÷åñòâà êóïþð äëÿ ïîëó÷åíèÿ íàëè÷íûõ
	private myTextBox tbGet10Rub;//10 ðóáëåâêè
	private myTextBox tbGet50Rub;//50 ðóáëåâêè
	private myTextBox tbGet100Rub;//100 ðóáëåâêè
	private myTextBox tbGet500Rub;//500 ðóáëåâêè
	private myTextBox tbGet1000Rub;//1000 ðóáëåâêè
	private myTextBox tbGet5000Rub;//5000 ðóáëåâêè
	
	public GetCashExchangeForm()
	{
		Text="Ïîëó÷åíèå íàëè÷íûõ ñðåäñòâ ñ ðàçìåíîì";//Çàãîëîâîê îêíà ïîëó÷åíèÿ íàëè÷íûõ ñ ðàçìåíîì

		//Ìåòêà - ïðèãëàøåíèå
		Label labelInvitation = new Label();
		labelInvitation.Text = "Óêàæèòå êîëè÷åñòâî êóïþð äîñòîèíñòâîì:";
		labelInvitation.Location = new Point(15,15);
		labelInvitation.AutoSize=true;
		labelInvitation.TextAlign=ContentAlignment.BottomLeft;
		this.Controls.Add(labelInvitation);

		//Ìåòêà äëÿ ïîëÿ ïîëó÷åíèÿ 10 ðóáëåâûõ êóïþð
		Label labelGet10Rub = new Label();
		labelGet10Rub.Text = "10 ðóá.:";
		labelGet10Rub.Location = new Point(15,labelInvitation.Height+25);
		labelGet10Rub.AutoSize=true;
		labelGet10Rub.TextAlign=ContentAlignment.BottomLeft;
		this.Controls.Add(labelGet10Rub);
		
		//Textbox ïîëó÷åíèÿ 10 ðóáëåâûõ êóïþð
		tbGet10Rub=new myTextBox();
		tbGet10Rub.Text = "";
		tbGet10Rub.Location=new Point(labelGet10Rub.Width+17,labelInvitation.Height+25);
		tbGet10Rub.Width=30;
		tbGet10Rub.Height=labelGet10Rub.Height;
		tbGet10Rub.TextAlign=HorizontalAlignment.Right;
		this.Controls.Add(tbGet10Rub);

		//Ìåòêà äëÿ ïîëÿ ïîëó÷åíèÿ 50 ðóáëåâûõ êóïþð
		Label labelGet50Rub = new Label();
		labelGet50Rub.Text = "50 ðóá.:";
		labelGet50Rub.Location = new Point(15,tbGet10Rub.Location.Y+tbGet10Rub.Height+2);
		labelGet50Rub.AutoSize=true;
		labelGet50Rub.TextAlign=ContentAlignment.BottomLeft;
		this.Controls.Add(labelGet50Rub);
		
		//Textbox ïîëó÷åíèÿ 50 ðóáëåâûõ êóïþð
		tbGet50Rub=new myTextBox();
		tbGet50Rub.Text = "";
		tbGet50Rub.Location=new Point(labelGet50Rub.Width+17,tbGet10Rub.Location.Y+tbGet10Rub.Height+2);
		tbGet50Rub.Width=30;
		tbGet50Rub.Height=labelGet50Rub.Height;
		tbGet50Rub.TextAlign=HorizontalAlignment.Right;
		this.Controls.Add(tbGet50Rub);
		
		//Ìåòêà äëÿ ïîëÿ ïîëó÷åíèÿ 100 ðóáëåâûõ êóïþð
		Label labelGet100Rub = new Label();
		labelGet100Rub.Text = "100 ðóá.:";
		labelGet100Rub.Location = new Point(15,tbGet50Rub.Location.Y+tbGet50Rub.Height+2);
		labelGet100Rub.AutoSize=true;
		labelGet100Rub.TextAlign=ContentAlignment.BottomLeft;
		this.Controls.Add(labelGet100Rub);
		
		//Textbox ïîëó÷åíèÿ 100 ðóáëåâûõ êóïþð
		tbGet100Rub=new myTextBox();
		tbGet100Rub.Text = "";
		tbGet100Rub.Location=new Point(labelGet100Rub.Width+17,tbGet50Rub.Location.Y+tbGet50Rub.Height+2);
		tbGet100Rub.Width=30;
		tbGet100Rub.Height=labelGet100Rub.Height;
		tbGet100Rub.TextAlign=HorizontalAlignment.Right;
		this.Controls.Add(tbGet100Rub);
		
		//Ìåòêà äëÿ ïîëÿ ïîëó÷åíèÿ 500 ðóáëåâûõ êóïþð
		Label labelGet500Rub = new Label();
		labelGet500Rub.Text = "500 ðóá.:";
		labelGet500Rub.Location = new Point(15,tbGet100Rub.Location.Y+tbGet100Rub.Height+2);
		labelGet500Rub.AutoSize=true;
		labelGet500Rub.TextAlign=ContentAlignment.BottomLeft;
		this.Controls.Add(labelGet500Rub);
		
		//Textbox ïîëó÷åíèÿ 500 ðóáëåâûõ êóïþð
		tbGet500Rub=new myTextBox();
		tbGet500Rub.Text = "";
		tbGet500Rub.Location=new Point(labelGet500Rub.Width+17,tbGet100Rub.Location.Y+tbGet100Rub.Height+2);
		tbGet500Rub.Width=30;
		tbGet500Rub.Height=labelGet500Rub.Height;
		tbGet500Rub.TextAlign=HorizontalAlignment.Right;
		this.Controls.Add(tbGet500Rub);
		
		//Ìåòêà äëÿ ïîëÿ ïîëó÷åíèÿ 1000 ðóáëåâûõ êóïþð
		Label labelGet1000Rub = new Label();
		labelGet1000Rub.Text = "1000 ðóá.:";
		labelGet1000Rub.Location = new Point(15,tbGet500Rub.Location.Y+tbGet500Rub.Height+2);
		labelGet1000Rub.AutoSize=true;
		labelGet1000Rub.TextAlign=ContentAlignment.BottomLeft;
		this.Controls.Add(labelGet1000Rub);
		
		//Textbox ïîëó÷åíèÿ 1000 ðóáëåâûõ êóïþð
		tbGet1000Rub=new myTextBox();
		tbGet1000Rub.Text = "";
		tbGet1000Rub.Location=new Point(labelGet1000Rub.Width+17,tbGet500Rub.Location.Y+tbGet500Rub.Height+2);
		tbGet1000Rub.Width=30;
		tbGet1000Rub.Height=labelGet1000Rub.Height;
		tbGet1000Rub.TextAlign=HorizontalAlignment.Right;
		this.Controls.Add(tbGet1000Rub);
		
		//Ìåòêà äëÿ ïîëÿ ïîëó÷åíèÿ 5000 ðóáëåâûõ êóïþð
		Label labelGet5000Rub = new Label();
		labelGet5000Rub.Text = "5000 ðóá.:";
		labelGet5000Rub.Location = new Point(15,tbGet1000Rub.Location.Y+tbGet1000Rub.Height+2);
		labelGet5000Rub.AutoSize=true;
		labelGet5000Rub.TextAlign=ContentAlignment.BottomLeft;
		this.Controls.Add(labelGet5000Rub);
		
		//Textbox ïîëó÷åíèÿ 5000 ðóáëåâûõ êóïþð
		tbGet5000Rub=new myTextBox();
		tbGet5000Rub.Text = "";
		tbGet5000Rub.Location=new Point(labelGet5000Rub.Width+17,tbGet1000Rub.Location.Y+tbGet1000Rub.Height+2);
		tbGet5000Rub.Width=30;
		tbGet5000Rub.Height=labelGet5000Rub.Height;
		tbGet5000Rub.TextAlign=HorizontalAlignment.Right;
		this.Controls.Add(tbGet5000Rub);
		
		//Êíîïêà ïîëó÷åíèÿ ñðåäñòâ
		Button bGet=new Button();
		bGet.Text="Ïîëó÷èòü";
		bGet.Location=new Point(15,tbGet5000Rub.Location.Y+tbGet1000Rub.Height+10);
		bGet.AutoSize=true;
		bGet.TextAlign=ContentAlignment.MiddleCenter;
		this.Controls.Add(bGet);
		bGet.Click += new System.EventHandler(bGet_Clicked);//Óñòàíîâêà îáðàáîò÷èêà íàæàòèÿ íà êíîïêó ïîëó÷åíèÿ ñðåäñòâ
	}

	//Ïîëó÷èòü ÷èñëåííîå çíà÷åíèå myTextBox
	public int getIntValMTB(myTextBox mTB)
	{
		return (mTB.Text=="")?0:Convert.ToInt32(mTB.Text);
	}
	
	//Îáðàáîò÷èê íàæàòèÿ íà êíîïêó ïîëó÷åíèÿ ñðåäñòâ
	protected void bGet_Clicked(object sender, EventArgs e)
	{
		//Çàïðàøèâàåìàÿ ñóììà â ãëàâíîé ôîðìå ïîëó÷åíèÿ íàëè÷íûõ
		int reqSum=getIntValMTB(AppForm.gcForm.tbGetCash);
		//Êîëè÷åñòâî çàïðàøèâàåìûõ êóïþð
		int numGet10Rub=getIntValMTB(tbGet10Rub);
		int numGet50Rub=getIntValMTB(tbGet50Rub);
		int numGet100Rub=getIntValMTB(tbGet100Rub);
		int numGet500Rub=getIntValMTB(tbGet500Rub);
		int numGet1000Rub=getIntValMTB(tbGet1000Rub);
		int numGet5000Rub=getIntValMTB(tbGet5000Rub);
		//Çàïðàøèâàåìàÿ ñóììà â ôîðìå ïîëó÷åíèÿ íàëè÷íûõ ñ ðàçìåíîì
		int reqExchangeSum=numGet10Rub*10+numGet50Rub*50+numGet100Rub*100+numGet500Rub*500+numGet1000Rub*1000+numGet5000Rub*5000;
		//Êîëè÷åñòâî êóïþð â áàíêîìàòå äî ïîëó÷åíèÿ
		int billsBeforeGet10=MyForm.appInstance.getStateOfATM()[d10].amount;
		int billsBeforeGet50=MyForm.appInstance.getStateOfATM()[d50].amount;
		int billsBeforeGet100=MyForm.appInstance.getStateOfATM()[d100].amount;
		int billsBeforeGet500=MyForm.appInstance.getStateOfATM()[d500].amount;
		int billsBeforeGet1000=MyForm.appInstance.getStateOfATM()[d1000].amount;
		int billsBeforeGet5000=MyForm.appInstance.getStateOfATM()[d5000].amount;		
		//Ïðåäïîëàãàåìîå êîëè÷åñòâî êóïþð â áàíêîìàòå ïîñëå ïîëó÷åíèÿ
		int billsAfterGet10=billsBeforeGet10-numGet10Rub;
		int billsAfterGet50=billsBeforeGet50-numGet50Rub;
		int billsAfterGet100=billsBeforeGet100-numGet100Rub;
		int billsAfterGet500=billsBeforeGet500-numGet500Rub;
		int billsAfterGet1000=billsBeforeGet1000-numGet1000Rub;
		int billsAfterGet5000=billsBeforeGet5000-numGet5000Rub;
		
		//Åñëè íå çàïîëíåíî íè îäíî ïîëå èëè ââåäåíû íóëè
		if(reqExchangeSum==0)
		{
			return;
		}
		//Åñëè çàïðàøèâàåìàÿ ñóììà â ôîðìå ïîëó÷åíèÿ íàëè÷íûõ ñ ðàçìåíîì íå ñîîòâåòñòâóåò çàïðàøèâàåìîé ñóììå â ãëàâíîé ôîðìå ïîëó÷åíèÿ íàëè÷íûõ
		if(reqExchangeSum!=reqSum)
		{
			MessageBox.Show("Ââåäåííîå êîëè÷åñòâî êóïþð íå ñîîòâåòñòâóåò çàïðàøèâàåìîé ñóììå.\nÂâåäèòå äðóãîå êîëè÷åñòâî","Ïîëó÷åíèå íàëè÷íûõ ñðåäñòâ ñ ðàçìåíîì");
			return;
		}
		//Åñëè êóïþð êàêîãî-ëèáî äîñòîèíñòâà â áàíêîìàòå íåäîñòàòî÷íî
		if(billsAfterGet10<0 ||
			billsAfterGet50<0 ||
			billsAfterGet100<0 ||
			billsAfterGet500<0 ||
			billsAfterGet1000<0 ||
			billsAfterGet5000<0)
		{
			MessageBox.Show("Ââåäåííîå êîëè÷åñòâî êóïþð ïðåâûøàåò èìåþùååñÿ.\nÂâåäèòå äðóãîå êîëè÷åñòâî","Ïîëó÷åíèå íàëè÷íûõ ñðåäñòâ ñ ðàçìåíîì");
			return;
		}
		
		//Èçúÿòèå 10 ðóáëåâûõ êóïþð èç áàíêîìàòà
		MyForm.appInstance.setBillsInATM(d10, billsAfterGet10);
		//Èçúÿòèå 50 ðóáëåâûõ êóïþð èç áàíêîìàòà
		MyForm.appInstance.setBillsInATM(d50, billsAfterGet50);
		//Èçúÿòèå 100 ðóáëåâûõ êóïþð èç áàíêîìàòà
		MyForm.appInstance.setBillsInATM(d100, billsAfterGet100);
		//Èçúÿòèå 500 ðóáëåâûõ êóïþð èç áàíêîìàòà
		MyForm.appInstance.setBillsInATM(d500, billsAfterGet500);
		//Èçúÿòèå 1000 ðóáëåâûõ êóïþð èç áàíêîìàòà
		MyForm.appInstance.setBillsInATM(d1000, billsAfterGet1000);
		//Èçúÿòèå 5000 ðóáëåâûõ êóïþð èç áàíêîìàòà
		MyForm.appInstance.setBillsInATM(d5000, billsAfterGet5000);
		
		//Ñòðîêîâûå âñòàâêè â ñîîáùåíèå êëèåíòó î êîëè÷åñòâå è íîìèíàëå ïîëó÷àåìûõ êóïþð
		string strNumGet10Rub=(numGet10Rub>0)?("\n"+numGet10Rub.ToString()+" - 10 ðóáëåâûõ êóïþð"):"";
		string strNumGet50Rub=(numGet50Rub>0)?("\n"+numGet50Rub.ToString()+" - 50 ðóáëåâûõ êóïþð"):"";
		string strNumGet100Rub=(numGet100Rub>0)?("\n"+numGet100Rub.ToString()+" - 100 ðóáëåâûõ êóïþð"):"";
		string strNumGet500Rub=(numGet500Rub>0)?("\n"+numGet500Rub.ToString()+" - 500 ðóáëåâûõ êóïþð"):"";
		string strNumGet1000Rub=(numGet1000Rub>0)?("\n"+numGet1000Rub.ToString()+" - 1000 ðóáëåâûõ êóïþð"):"";
		string strNumGet5000Rub=(numGet5000Rub>0)?("\n"+numGet5000Rub.ToString()+" - 5000 ðóáëåâûõ êóïþð"):"";
		
		MessageBox.Show("Ïîëó÷èòå "+(reqExchangeSum).ToString()+" ðóá.:"+strNumGet10Rub+strNumGet50Rub+strNumGet100Rub+strNumGet500Rub+strNumGet1000Rub+strNumGet5000Rub,
		"Ïîëó÷åíèå íàëè÷íûõ ñðåäñòâ ñ ðàçìåíîì");
		
		this.Hide();
	}
}

//Êëàññ òåêñòîâîãî ïîëÿ ââîäà öåëûõ ÷èñåë
class myTextBox : TextBox
{
	//Ïåðåãðóçêà ôóíêöèè îáðàáîòêè ñîáûòèÿ èçìåíåíèÿ òåêñòà â TextBox
	protected override void OnTextChanged(EventArgs e)
	{
		string s=getMyTBStr();
		for(int i=0; i<s.Length; i++)
		{
			if(s[i]<'0' || s[i]>'9')
			{
				MessageBox.Show("Ââåäèòå íåîòðèöàòåëüíîå öåëîå ÷èñëî","Ìèíèìàëüíîå êîëè÷åñòâî áóêâ â ñëîâàõ");
				this.Text="";
			}
		}		
	}

	//Ïîëó÷èòü òåêñò â ïîëå ââîäà ìèíèìàëüíîãî êîëè÷åñòâà áóêâ â ñëîâàõ
	public string getMyTBStr()
	{
		return this.Text;
	}	
}
