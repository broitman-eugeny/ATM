//�������� ������� ������������ C#�
//������� �2
//��������� ��������

//������������ ����
using System;//����� ���������� ����� �������� �������, � �� ����� ������� ���� �����
using System.Windows.Forms;//��� ������ � �������
using System.Drawing;//������ � �������� �������� GDI
//using System.IO;//��������� ���� ����� (������ � �������)
//using System.Text;//��� Encoding
//using System.Windows.Controls;//��� TextChangedEventArgs

class MyForm : AppForm 
{
	public static MyForm appInstance;
		
	// Satisfies rule: MarkWindowsFormsEntryPointsWithStaThread.
	[STAThread]//������������ ���������� (��� ����������� ������������� ����������� Windows Forms)
	public static void Main()
	{
		appInstance=new MyForm();
		Application.Run(appInstance);
	}
}

//��������� ��������� ��������� ��� ������ ����������� �����
struct inATM
{
	public int amount;//���������� ����� ������� �����������, ��������� � ���������
	public int maxAmount;//����������� ��������� ���������� ����� ������� �����������, ���������� � ��������
	public Label lbl;//����� ���������� �����, ��������� � ���������	
}

//����� ����������
class AppForm : Form
{
	//������� ��������� � ������� stateOfATM � ������������ � ������������ �����
	const int d10=0;
	const int d50=1;
	const int d100=2;
	const int d500=3;
	const int d1000=4;
	const int d5000=5;
	//������ ��������� ��������� (���������� � ����������� �����)
	private inATM[] stateOfATM = new inATM[6];
	
	//��������� ������ ������� ����� ��������� ��������
	public static GetCashForm gcForm;
	
	//������� ��������� ��������� ���������
	public inATM[] getStateOfATM()
	{
		return this.stateOfATM;
	}
	
	//������� ��������� ���������� ����� ��������� ����������� � ��������� � ��������������� ����� ��������� ���������
	//digIndex - ������ ����������� �����
	//amount - ���������� ����� ������� �����������
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
			MessageBox.Show("����� ����� ��������� ����������� ����������","������ "+s+" ���.");
			return;
		}
		this.stateOfATM[digIndex].amount=amount;		
		this.stateOfATM[digIndex].lbl.Text=s+" ���.: "+amount.ToString()+" ��. �� ����.="+this.stateOfATM[digIndex].maxAmount.ToString()+" ��.";
	}

	//������� ��������� ����������� ���������� ���������� ����� ������� �����������, ����������� � ��������
	//digIndex - ������ ����������� ����� � ������������ � enum digIndexes
	//maxAmount - ����������� ��������� ���������� ����� ������� �����������
	public void setMaxBillsInATM(int digIndex, int maxAmount)
	{
		this.stateOfATM[digIndex].maxAmount=maxAmount;		
	}
	
	public AppForm()
	{
		this.Text="��������";//��������� ���� ���������
		this.Width=250;//������ ����
		this.Height=250;//������ ����

		//���� ��������
		MainMenu mnuFileMenu = new MainMenu();
		this.Menu = mnuFileMenu;
		MenuItem MenuItemFile = new MenuItem("&File");
		MenuItemFile.MenuItems.Add("E&xit", new System.EventHandler(this.MenuExit_Click));
		MenuItem MenuItemOperations = new MenuItem("&Operations");
		MenuItemOperations.MenuItems.Add("&Contribute to ATM",new System.EventHandler(this.MenuContribute_Click));
		MenuItemOperations.MenuItems.Add("&Get cash from ATM",new System.EventHandler(this.MenuGet_Click));
		mnuFileMenu.MenuItems.Add(MenuItemFile);
		mnuFileMenu.MenuItems.Add(MenuItemOperations);

		//������������� ����� ��������� ��������� (���������� ��������� �����, ������������ ����������)
		//����� �������
		Label labelInATM = new Label();
		labelInATM.Text = "������� ����� � ���������:";
		labelInATM.Location = new Point(15,15);
		labelInATM.AutoSize=true;
		labelInATM.TextAlign=ContentAlignment.BottomLeft;
		this.Controls.Add(labelInATM);
		int vShift=10;//�������������� ������������ ����� ����� �������
		//����� ��������� ��������� �� ������������ �����
		for(int i=d10; i<=d5000; i++)
		{
			setMaxBillsInATM(i, 1000);
			this.stateOfATM[i].lbl=new Label();
			setBillsInATM(i, 0);
			if(i>d10)//������� ����� �� ����������� �����
			{
				vShift+=this.stateOfATM[i-1].lbl.Height+2;
			}
			this.stateOfATM[i].lbl.Location = new Point(15,labelInATM.Location.Y+labelInATM.Height+vShift);
			this.stateOfATM[i].lbl.AutoSize=true;
			this.stateOfATM[i].lbl.TextAlign=ContentAlignment.BottomLeft;
			this.Controls.Add(this.stateOfATM[i].lbl);
		}	
	}
	
	//��� ������ ���� File\Exit
	private void MenuExit_Click(Object sender, EventArgs e)
	{
   		Application.Exit();
	}
	
	//��� ������ ���� Operations\Contribute to ATM
	public void MenuContribute_Click(Object sender, EventArgs e)
	{
		ContribForm cForm=new ContribForm();
		cForm.ShowDialog(this);
		cForm.Dispose();
	}
	
	//��� ������ ���� Operations\Get cash from ATM
	private void MenuGet_Click(Object sender, EventArgs e)
	{
		gcForm=new GetCashForm();
		gcForm.ShowDialog(this);
		gcForm.Dispose();
	}
}



//����� �������� �������
class ContribForm : Form
{
	//������� ��������� � ������� stateOfATM � ������������ � ������������ �����
	const int d10=0;
	const int d50=1;
	const int d100=2;
	const int d500=3;
	const int d1000=4;
	const int d5000=5;
	
	//�������� ����� ���������� ����� ��� �������� � ��������
	private myTextBox tbContrib10Rub;//10 ��������
	private myTextBox tbContrib50Rub;//50 ��������
	private myTextBox tbContrib100Rub;//100 ��������
	private myTextBox tbContrib500Rub;//500 ��������
	private myTextBox tbContrib1000Rub;//1000 ��������
	private myTextBox tbContrib5000Rub;//5000 ��������
	
	public ContribForm()
	{
		Text="�������� �������� ������� � ��������";//��������� ���� �������� �������

		//����� - �����������
		Label labelInvitation = new Label();
		labelInvitation.Text = "������� ���������� �������� ����� ������������:";
		labelInvitation.Location = new Point(15,15);
		labelInvitation.AutoSize=true;
		labelInvitation.TextAlign=ContentAlignment.BottomLeft;
		this.Controls.Add(labelInvitation);

		//����� ��� ���� �������� 10 �������� �����
		Label labelContrib10Rub = new Label();
		labelContrib10Rub.Text = "10 ���.:";
		labelContrib10Rub.Location = new Point(15,labelInvitation.Height+25);
		labelContrib10Rub.AutoSize=true;
		labelContrib10Rub.TextAlign=ContentAlignment.BottomLeft;
		this.Controls.Add(labelContrib10Rub);
		
		//Textbox �������� 10 �������� �����
		tbContrib10Rub=new myTextBox();
		tbContrib10Rub.Text = "";
		tbContrib10Rub.Location=new Point(labelContrib10Rub.Width+17,labelInvitation.Height+25);
		tbContrib10Rub.Width=30;
		tbContrib10Rub.Height=labelContrib10Rub.Height;
		tbContrib10Rub.TextAlign=HorizontalAlignment.Right;
		this.Controls.Add(tbContrib10Rub);

		//����� ��� ���� �������� 50 �������� �����
		Label labelContrib50Rub = new Label();
		labelContrib50Rub.Text = "50 ���.:";
		labelContrib50Rub.Location = new Point(15,tbContrib10Rub.Location.Y+tbContrib10Rub.Height+2);
		labelContrib50Rub.AutoSize=true;
		labelContrib50Rub.TextAlign=ContentAlignment.BottomLeft;
		this.Controls.Add(labelContrib50Rub);
		
		//Textbox �������� 50 �������� �����
		tbContrib50Rub=new myTextBox();
		tbContrib50Rub.Text = "";
		tbContrib50Rub.Location=new Point(labelContrib50Rub.Width+17,tbContrib10Rub.Location.Y+tbContrib10Rub.Height+2);
		tbContrib50Rub.Width=30;
		tbContrib50Rub.Height=labelContrib50Rub.Height;
		tbContrib50Rub.TextAlign=HorizontalAlignment.Right;
		this.Controls.Add(tbContrib50Rub);
		
		//����� ��� ���� �������� 100 �������� �����
		Label labelContrib100Rub = new Label();
		labelContrib100Rub.Text = "100 ���.:";
		labelContrib100Rub.Location = new Point(15,tbContrib50Rub.Location.Y+tbContrib50Rub.Height+2);
		labelContrib100Rub.AutoSize=true;
		labelContrib100Rub.TextAlign=ContentAlignment.BottomLeft;
		this.Controls.Add(labelContrib100Rub);
		
		//Textbox �������� 100 �������� �����
		tbContrib100Rub=new myTextBox();
		tbContrib100Rub.Text = "";
		tbContrib100Rub.Location=new Point(labelContrib100Rub.Width+17,tbContrib50Rub.Location.Y+tbContrib50Rub.Height+2);
		tbContrib100Rub.Width=30;
		tbContrib100Rub.Height=labelContrib100Rub.Height;
		tbContrib100Rub.TextAlign=HorizontalAlignment.Right;
		this.Controls.Add(tbContrib100Rub);
		
		//����� ��� ���� �������� 500 �������� �����
		Label labelContrib500Rub = new Label();
		labelContrib500Rub.Text = "500 ���.:";
		labelContrib500Rub.Location = new Point(15,tbContrib100Rub.Location.Y+tbContrib100Rub.Height+2);
		labelContrib500Rub.AutoSize=true;
		labelContrib500Rub.TextAlign=ContentAlignment.BottomLeft;
		this.Controls.Add(labelContrib500Rub);
		
		//Textbox �������� 500 �������� �����
		tbContrib500Rub=new myTextBox();
		tbContrib500Rub.Text = "";
		tbContrib500Rub.Location=new Point(labelContrib500Rub.Width+17,tbContrib100Rub.Location.Y+tbContrib100Rub.Height+2);
		tbContrib500Rub.Width=30;
		tbContrib500Rub.Height=labelContrib500Rub.Height;
		tbContrib500Rub.TextAlign=HorizontalAlignment.Right;
		this.Controls.Add(tbContrib500Rub);
		
		//����� ��� ���� �������� 1000 �������� �����
		Label labelContrib1000Rub = new Label();
		labelContrib1000Rub.Text = "1000 ���.:";
		labelContrib1000Rub.Location = new Point(15,tbContrib500Rub.Location.Y+tbContrib500Rub.Height+2);
		labelContrib1000Rub.AutoSize=true;
		labelContrib1000Rub.TextAlign=ContentAlignment.BottomLeft;
		this.Controls.Add(labelContrib1000Rub);
		
		//Textbox �������� 1000 �������� �����
		tbContrib1000Rub=new myTextBox();
		tbContrib1000Rub.Text = "";
		tbContrib1000Rub.Location=new Point(labelContrib1000Rub.Width+17,tbContrib500Rub.Location.Y+tbContrib500Rub.Height+2);
		tbContrib1000Rub.Width=30;
		tbContrib1000Rub.Height=labelContrib1000Rub.Height;
		tbContrib1000Rub.TextAlign=HorizontalAlignment.Right;
		this.Controls.Add(tbContrib1000Rub);
		
		//����� ��� ���� �������� 5000 �������� �����
		Label labelContrib5000Rub = new Label();
		labelContrib5000Rub.Text = "5000 ���.:";
		labelContrib5000Rub.Location = new Point(15,tbContrib1000Rub.Location.Y+tbContrib1000Rub.Height+2);
		labelContrib5000Rub.AutoSize=true;
		labelContrib5000Rub.TextAlign=ContentAlignment.BottomLeft;
		this.Controls.Add(labelContrib5000Rub);
		
		//Textbox �������� 5000 �������� �����
		tbContrib5000Rub=new myTextBox();
		tbContrib5000Rub.Text = "";
		tbContrib5000Rub.Location=new Point(labelContrib5000Rub.Width+17,tbContrib1000Rub.Location.Y+tbContrib1000Rub.Height+2);
		tbContrib5000Rub.Width=30;
		tbContrib5000Rub.Height=labelContrib5000Rub.Height;
		tbContrib5000Rub.TextAlign=HorizontalAlignment.Right;
		this.Controls.Add(tbContrib5000Rub);
		
		//������ �������� �������
		Button bContrib=new Button();
		bContrib.Text="������";
		bContrib.Location=new Point(15,tbContrib5000Rub.Location.Y+tbContrib1000Rub.Height+10);
		bContrib.AutoSize=true;
		bContrib.TextAlign=ContentAlignment.MiddleCenter;
		this.Controls.Add(bContrib);
		bContrib.Click += new System.EventHandler(bContrib_Clicked);//��������� ����������� ������� �� ������ �������� �������
	}

	//�������� ��������� �������� myTextBox
	public int getIntValMTB(myTextBox mTB)
	{
		return (mTB.Text=="")?0:Convert.ToInt32(mTB.Text);
	}
	
	//���������� ������� �� ������ �������� �������
	protected void bContrib_Clicked(object sender, EventArgs e)
	{
		//���������� ����� � ��������� �� ����������
		int billsBeforeContrib10=MyForm.appInstance.getStateOfATM()[d10].amount;
		int billsBeforeContrib50=MyForm.appInstance.getStateOfATM()[d50].amount;
		int billsBeforeContrib100=MyForm.appInstance.getStateOfATM()[d100].amount;
		int billsBeforeContrib500=MyForm.appInstance.getStateOfATM()[d500].amount;
		int billsBeforeContrib1000=MyForm.appInstance.getStateOfATM()[d1000].amount;
		int billsBeforeContrib5000=MyForm.appInstance.getStateOfATM()[d5000].amount;		
		//�������������� ���������� ����� � ��������� ����� ����������
		int billsAfterContrib10=billsBeforeContrib10+getIntValMTB(tbContrib10Rub);
		int billsAfterContrib50=billsBeforeContrib50+getIntValMTB(tbContrib50Rub);
		int billsAfterContrib100=billsBeforeContrib100+getIntValMTB(tbContrib100Rub);
		int billsAfterContrib500=billsBeforeContrib500+getIntValMTB(tbContrib500Rub);
		int billsAfterContrib1000=billsBeforeContrib1000+getIntValMTB(tbContrib1000Rub);
		int billsAfterContrib5000=billsBeforeContrib5000+getIntValMTB(tbContrib5000Rub);
		
		//�������� 10 �������� �����
		MyForm.appInstance.setBillsInATM(d10, billsAfterContrib10);
		//�������� 50 �������� �����
		MyForm.appInstance.setBillsInATM(d50, billsAfterContrib50);
		//�������� 100 �������� �����
		MyForm.appInstance.setBillsInATM(d100, billsAfterContrib100);
		//�������� 500 �������� �����
		MyForm.appInstance.setBillsInATM(d500, billsAfterContrib500);
		//�������� 1000 �������� �����
		MyForm.appInstance.setBillsInATM(d1000, billsAfterContrib1000);
		//�������� 5000 �������� �����
		MyForm.appInstance.setBillsInATM(d5000, billsAfterContrib5000);
		if(billsAfterContrib10>MyForm.appInstance.getStateOfATM()[d10].maxAmount ||//���������� ����� ���� �� ����� ��������� ����������� ���������� � ���������
			billsAfterContrib50>MyForm.appInstance.getStateOfATM()[d50].maxAmount ||
			billsAfterContrib100>MyForm.appInstance.getStateOfATM()[d100].maxAmount ||
			billsAfterContrib500>MyForm.appInstance.getStateOfATM()[d500].maxAmount ||
			billsAfterContrib1000>MyForm.appInstance.getStateOfATM()[d1000].maxAmount ||
			billsAfterContrib5000>MyForm.appInstance.getStateOfATM()[d5000].maxAmount)
		{
			if(!(MyForm.appInstance.getStateOfATM()[d10].amount==billsBeforeContrib10 &&//����� ���� �� ����� ������� � ��������
			MyForm.appInstance.getStateOfATM()[d50].amount==billsBeforeContrib50 &&
			MyForm.appInstance.getStateOfATM()[d100].amount==billsBeforeContrib100 &&
			MyForm.appInstance.getStateOfATM()[d500].amount==billsBeforeContrib500 &&
			MyForm.appInstance.getStateOfATM()[d1000].amount==billsBeforeContrib1000 &&
			MyForm.appInstance.getStateOfATM()[d5000].amount==billsBeforeContrib5000))
			{
				MessageBox.Show("��������� ������ ������� �������", "�������� �������");
			}			
		}
		else
		{
			if(!(tbContrib10Rub.Text=="" &&//����� ���� �� ����� ������� � ��������
			tbContrib50Rub.Text=="" &&
			tbContrib100Rub.Text=="" &&
			tbContrib500Rub.Text=="" &&
			tbContrib1000Rub.Text=="" &&
			tbContrib5000Rub.Text==""))
			{
				MessageBox.Show("������ ������� �������", "�������� �������");
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

//������� ����� ��������� ��������
class GetCashForm : Form
{
	//������� ��������� � ������� stateOfATM � ������������ � ������������ �����
	const int d10=0;
	const int d50=1;
	const int d100=2;
	const int d500=3;
	const int d1000=4;
	const int d5000=5;
	
	//������� ����� ������������� �����
	public myTextBox tbGetCash;
	//�������� ������ �������/������ �����
	private RadioButton rbLargeBills;//������� ������
	private RadioButton rbSmallBills;//������ ������
	
	public GetCashForm()
	{
		Text="��������� �������� �������";//��������� ���� ��������� �������
		
		//����� - �����������
		Label labelInvitation = new Label();
		labelInvitation.Text = "������� ��������� ����� � ������:";
		labelInvitation.Location = new Point(15,15);
		labelInvitation.AutoSize=true;
		labelInvitation.TextAlign=ContentAlignment.BottomLeft;
		this.Controls.Add(labelInvitation);
		
		//Textbox ��� ����� ������������� �����
		tbGetCash=new myTextBox();
		tbGetCash.Text = "";
		tbGetCash.Location=new Point(labelInvitation.Width+17,15);
		tbGetCash.Width=50;
		tbGetCash.Height=labelInvitation.Height;
		tbGetCash.TextAlign=HorizontalAlignment.Right;
		this.Controls.Add(tbGetCash);
		
		//GroupBox ��� ������ �������/������ �����
		GroupBox gbLargeSmallBills = new GroupBox();		
		rbLargeBills = new RadioButton();
		rbSmallBills = new RadioButton();
		gbLargeSmallBills.Controls.Add(rbLargeBills);
		gbLargeSmallBills.Controls.Add(rbSmallBills);
		gbLargeSmallBills.Location = new Point(15, tbGetCash.Height+17);
		gbLargeSmallBills.Width=rbLargeBills.Width+rbSmallBills.Width+32;
		gbLargeSmallBills.Height=rbLargeBills.Height+30;
		gbLargeSmallBills.Text = "������ �������� ������ �����?";
		rbLargeBills.Text = "��������";
		rbSmallBills.Text = "� ��������";
		rbLargeBills.Location = new Point(15, 20);
		rbSmallBills.Location = new Point(rbLargeBills.Width+17, 20);
		rbLargeBills.AutoSize=true;
		rbSmallBills.AutoSize=true;
		Controls.Add(gbLargeSmallBills);
		rbLargeBills.Checked=true;
		
		//������ ��������� ��������
		Button bGet=new Button();
		bGet.Text="��������";
		bGet.Location=new Point(15,gbLargeSmallBills.Height+tbGetCash.Height+19);
		bGet.AutoSize=true;
		bGet.TextAlign=ContentAlignment.MiddleCenter;
		this.Controls.Add(bGet);
		bGet.Click += new System.EventHandler(bGet_Clicked);//��������� ����������� ������� �� ������ ��������� ��������
	}
	
	//�������� ��������� �������� myTextBox
	public int getIntValMTB(myTextBox mTB)
	{
		return (mTB.Text=="")?0:Convert.ToInt32(mTB.Text);
	}
	
	//���������� ������� �� ������ ��������� ��������
	protected void bGet_Clicked(object sender, EventArgs e)
	{
		//���������� ����� ���������� ����������
		int numBills5000;
		int numBills1000;
		int numBills500;
		int numBills100;
		int numBills50;
		int numBills10;
		//���������� ����� � ��������� �� ��������� ��������
		int billsBeforeGet10=MyForm.appInstance.getStateOfATM()[d10].amount;
		int billsBeforeGet50=MyForm.appInstance.getStateOfATM()[d50].amount;
		int billsBeforeGet100=MyForm.appInstance.getStateOfATM()[d100].amount;
		int billsBeforeGet500=MyForm.appInstance.getStateOfATM()[d500].amount;
		int billsBeforeGet1000=MyForm.appInstance.getStateOfATM()[d1000].amount;
		int billsBeforeGet5000=MyForm.appInstance.getStateOfATM()[d5000].amount;
		//��������� �����
		int sumReq=getIntValMTB(tbGetCash);		
		//���������� ���������� ����� ��� ������ ��������� �����
		if(sumReq>0)//��������� ����� �� �������
		{
			if(rbLargeBills.Checked==true)//������� �������� ��������
			{
				numBills5000=sumReq/5000;//���������� 5000 �����, ��������� ��� ������������� �����
				if(billsBeforeGet5000<numBills5000)//���� � ��������� ��� ������� 5000 �����
				{
					numBills5000=billsBeforeGet5000;//�������� ��� �� ���������
				}
				int remains=sumReq-(5000*numBills5000);//������� ������������� ����� ����� ���������� ����� �������������� 5000 ��������
				numBills1000=remains/1000;//���������� 1000 �����, ��������� ��� ������� ������������� �����
				if(billsBeforeGet1000<numBills1000)//���� � ��������� ��� ������� 1000 �����
				{
					numBills1000=billsBeforeGet1000;//�������� ��� �� ���������
				}
				remains-=1000*numBills1000;//������� ������������� ����� ����� ���������� ����� �������������� 1000 ��������
				numBills500=remains/500;//���������� 500 �����, ��������� ��� ������� ������������� �����
				if(billsBeforeGet500<numBills500)//���� � ��������� ��� ������� 500 �����
				{
					numBills500=billsBeforeGet500;//�������� ��� �� ���������
				}
				remains-=500*numBills500;//������� ������������� ����� ����� ���������� ����� �������������� 500 ��������
				numBills100=remains/100;//���������� 100 �����, ��������� ��� ������� ������������� �����
				if(billsBeforeGet100<numBills100)//���� � ��������� ��� ������� 100 �����
				{
					numBills100=billsBeforeGet100;//�������� ��� �� ���������
				}
				remains-=100*numBills100;//������� ������������� ����� ����� ���������� ����� �������������� 100 ��������
				numBills50=remains/50;//���������� 50 �����, ��������� ��� ������� ������������� �����
				if(billsBeforeGet50<numBills50)//���� � ��������� ��� ������� 50 �����
				{
					numBills50=billsBeforeGet50;//�������� ��� �� ���������
				}
				remains-=50*numBills50;//������� ������������� ����� ����� ���������� ����� �������������� 50 ��������
				numBills10=remains/10;//���������� 10 �����, ��������� ��� ������� ������������� �����
				if(billsBeforeGet10<numBills10)//���� � ��������� ��� ������� 10 �����
				{
					MessageBox.Show("� ��������� ����������� ��������� ����� ���� ������ ���������� �����������.\n������� ������ ��������", "��������� �������� �������");
					return;
				}
				remains-=10*numBills10;//������� ������������� ����� ����� ���������� ����� �������������� 10 ��������
				if(remains>0)//����� �� ������ 10
				{
					MessageBox.Show("����������� ������ ������������ ������ 10 ���.\n������� �������� ������� 10", "��������� �������� �������");
					return;
				}
				//������� 10 �������� ����� �� ���������
				MyForm.appInstance.setBillsInATM(d10, billsBeforeGet10-numBills10);
				//������� 50 �������� ����� �� ���������
				MyForm.appInstance.setBillsInATM(d50, billsBeforeGet50-numBills50);
				//������� 100 �������� ����� �� ���������
				MyForm.appInstance.setBillsInATM(d100, billsBeforeGet100-numBills100);
				//������� 500 �������� ����� �� ���������
				MyForm.appInstance.setBillsInATM(d500, billsBeforeGet500-numBills500);
				//������� 1000 �������� ����� �� ���������
				MyForm.appInstance.setBillsInATM(d1000, billsBeforeGet1000-numBills1000);
				//������� 5000 �������� ����� �� ���������
				MyForm.appInstance.setBillsInATM(d5000, billsBeforeGet5000-numBills5000);
		
				//��������� ������� � ��������� ������� � ���������� � �������� ���������� �����
				string strNumGet10Rub=(numBills10>0)?("\n"+numBills10.ToString()+" - 10 �������� �����"):"";
				string strNumGet50Rub=(numBills50>0)?("\n"+numBills50.ToString()+" - 50 �������� �����"):"";
				string strNumGet100Rub=(numBills100>0)?("\n"+numBills100.ToString()+" - 100 �������� �����"):"";
				string strNumGet500Rub=(numBills500>0)?("\n"+numBills500.ToString()+" - 500 �������� �����"):"";
				string strNumGet1000Rub=(numBills1000>0)?("\n"+numBills1000.ToString()+" - 1000 �������� �����"):"";
				string strNumGet5000Rub=(numBills5000>0)?("\n"+numBills5000.ToString()+" - 5000 �������� �����"):"";
		
				MessageBox.Show("�������� "+(numBills10*10+numBills50*50+numBills100*100+numBills500*500+numBills1000*1000+numBills5000*5000).ToString()+" ���.:"+strNumGet10Rub+strNumGet50Rub+strNumGet100Rub+strNumGet500Rub+strNumGet1000Rub+strNumGet5000Rub,
				"��������� �������� ������� �������� ��������");				
			}
			else//������� �������� c ��������
			{
				GetCashExchangeForm gcEForm=new GetCashExchangeForm();
				gcEForm.ShowDialog(this);
				gcEForm.Dispose();
			}
			this.Hide();
		}
	}
}

//����� ��������� �������� � ��������
class GetCashExchangeForm : Form
{
	//������� ��������� � ������� stateOfATM � ������������ � ������������ �����
	const int d10=0;
	const int d50=1;
	const int d100=2;
	const int d500=3;
	const int d1000=4;
	const int d5000=5;
	
	//�������� ����� ���������� ����� ��� ��������� ��������
	private myTextBox tbGet10Rub;//10 ��������
	private myTextBox tbGet50Rub;//50 ��������
	private myTextBox tbGet100Rub;//100 ��������
	private myTextBox tbGet500Rub;//500 ��������
	private myTextBox tbGet1000Rub;//1000 ��������
	private myTextBox tbGet5000Rub;//5000 ��������
	
	public GetCashExchangeForm()
	{
		Text="��������� �������� ������� � ��������";//��������� ���� ��������� �������� � ��������

		//����� - �����������
		Label labelInvitation = new Label();
		labelInvitation.Text = "������� ���������� ����� ������������:";
		labelInvitation.Location = new Point(15,15);
		labelInvitation.AutoSize=true;
		labelInvitation.TextAlign=ContentAlignment.BottomLeft;
		this.Controls.Add(labelInvitation);

		//����� ��� ���� ��������� 10 �������� �����
		Label labelGet10Rub = new Label();
		labelGet10Rub.Text = "10 ���.:";
		labelGet10Rub.Location = new Point(15,labelInvitation.Height+25);
		labelGet10Rub.AutoSize=true;
		labelGet10Rub.TextAlign=ContentAlignment.BottomLeft;
		this.Controls.Add(labelGet10Rub);
		
		//Textbox ��������� 10 �������� �����
		tbGet10Rub=new myTextBox();
		tbGet10Rub.Text = "";
		tbGet10Rub.Location=new Point(labelGet10Rub.Width+17,labelInvitation.Height+25);
		tbGet10Rub.Width=30;
		tbGet10Rub.Height=labelGet10Rub.Height;
		tbGet10Rub.TextAlign=HorizontalAlignment.Right;
		this.Controls.Add(tbGet10Rub);

		//����� ��� ���� ��������� 50 �������� �����
		Label labelGet50Rub = new Label();
		labelGet50Rub.Text = "50 ���.:";
		labelGet50Rub.Location = new Point(15,tbGet10Rub.Location.Y+tbGet10Rub.Height+2);
		labelGet50Rub.AutoSize=true;
		labelGet50Rub.TextAlign=ContentAlignment.BottomLeft;
		this.Controls.Add(labelGet50Rub);
		
		//Textbox ��������� 50 �������� �����
		tbGet50Rub=new myTextBox();
		tbGet50Rub.Text = "";
		tbGet50Rub.Location=new Point(labelGet50Rub.Width+17,tbGet10Rub.Location.Y+tbGet10Rub.Height+2);
		tbGet50Rub.Width=30;
		tbGet50Rub.Height=labelGet50Rub.Height;
		tbGet50Rub.TextAlign=HorizontalAlignment.Right;
		this.Controls.Add(tbGet50Rub);
		
		//����� ��� ���� ��������� 100 �������� �����
		Label labelGet100Rub = new Label();
		labelGet100Rub.Text = "100 ���.:";
		labelGet100Rub.Location = new Point(15,tbGet50Rub.Location.Y+tbGet50Rub.Height+2);
		labelGet100Rub.AutoSize=true;
		labelGet100Rub.TextAlign=ContentAlignment.BottomLeft;
		this.Controls.Add(labelGet100Rub);
		
		//Textbox ��������� 100 �������� �����
		tbGet100Rub=new myTextBox();
		tbGet100Rub.Text = "";
		tbGet100Rub.Location=new Point(labelGet100Rub.Width+17,tbGet50Rub.Location.Y+tbGet50Rub.Height+2);
		tbGet100Rub.Width=30;
		tbGet100Rub.Height=labelGet100Rub.Height;
		tbGet100Rub.TextAlign=HorizontalAlignment.Right;
		this.Controls.Add(tbGet100Rub);
		
		//����� ��� ���� ��������� 500 �������� �����
		Label labelGet500Rub = new Label();
		labelGet500Rub.Text = "500 ���.:";
		labelGet500Rub.Location = new Point(15,tbGet100Rub.Location.Y+tbGet100Rub.Height+2);
		labelGet500Rub.AutoSize=true;
		labelGet500Rub.TextAlign=ContentAlignment.BottomLeft;
		this.Controls.Add(labelGet500Rub);
		
		//Textbox ��������� 500 �������� �����
		tbGet500Rub=new myTextBox();
		tbGet500Rub.Text = "";
		tbGet500Rub.Location=new Point(labelGet500Rub.Width+17,tbGet100Rub.Location.Y+tbGet100Rub.Height+2);
		tbGet500Rub.Width=30;
		tbGet500Rub.Height=labelGet500Rub.Height;
		tbGet500Rub.TextAlign=HorizontalAlignment.Right;
		this.Controls.Add(tbGet500Rub);
		
		//����� ��� ���� ��������� 1000 �������� �����
		Label labelGet1000Rub = new Label();
		labelGet1000Rub.Text = "1000 ���.:";
		labelGet1000Rub.Location = new Point(15,tbGet500Rub.Location.Y+tbGet500Rub.Height+2);
		labelGet1000Rub.AutoSize=true;
		labelGet1000Rub.TextAlign=ContentAlignment.BottomLeft;
		this.Controls.Add(labelGet1000Rub);
		
		//Textbox ��������� 1000 �������� �����
		tbGet1000Rub=new myTextBox();
		tbGet1000Rub.Text = "";
		tbGet1000Rub.Location=new Point(labelGet1000Rub.Width+17,tbGet500Rub.Location.Y+tbGet500Rub.Height+2);
		tbGet1000Rub.Width=30;
		tbGet1000Rub.Height=labelGet1000Rub.Height;
		tbGet1000Rub.TextAlign=HorizontalAlignment.Right;
		this.Controls.Add(tbGet1000Rub);
		
		//����� ��� ���� ��������� 5000 �������� �����
		Label labelGet5000Rub = new Label();
		labelGet5000Rub.Text = "5000 ���.:";
		labelGet5000Rub.Location = new Point(15,tbGet1000Rub.Location.Y+tbGet1000Rub.Height+2);
		labelGet5000Rub.AutoSize=true;
		labelGet5000Rub.TextAlign=ContentAlignment.BottomLeft;
		this.Controls.Add(labelGet5000Rub);
		
		//Textbox ��������� 5000 �������� �����
		tbGet5000Rub=new myTextBox();
		tbGet5000Rub.Text = "";
		tbGet5000Rub.Location=new Point(labelGet5000Rub.Width+17,tbGet1000Rub.Location.Y+tbGet1000Rub.Height+2);
		tbGet5000Rub.Width=30;
		tbGet5000Rub.Height=labelGet5000Rub.Height;
		tbGet5000Rub.TextAlign=HorizontalAlignment.Right;
		this.Controls.Add(tbGet5000Rub);
		
		//������ ��������� �������
		Button bGet=new Button();
		bGet.Text="��������";
		bGet.Location=new Point(15,tbGet5000Rub.Location.Y+tbGet1000Rub.Height+10);
		bGet.AutoSize=true;
		bGet.TextAlign=ContentAlignment.MiddleCenter;
		this.Controls.Add(bGet);
		bGet.Click += new System.EventHandler(bGet_Clicked);//��������� ����������� ������� �� ������ ��������� �������
	}

	//�������� ��������� �������� myTextBox
	public int getIntValMTB(myTextBox mTB)
	{
		return (mTB.Text=="")?0:Convert.ToInt32(mTB.Text);
	}
	
	//���������� ������� �� ������ ��������� �������
	protected void bGet_Clicked(object sender, EventArgs e)
	{
		//������������� ����� � ������� ����� ��������� ��������
		int reqSum=getIntValMTB(AppForm.gcForm.tbGetCash);
		//���������� ������������� �����
		int numGet10Rub=getIntValMTB(tbGet10Rub);
		int numGet50Rub=getIntValMTB(tbGet50Rub);
		int numGet100Rub=getIntValMTB(tbGet100Rub);
		int numGet500Rub=getIntValMTB(tbGet500Rub);
		int numGet1000Rub=getIntValMTB(tbGet1000Rub);
		int numGet5000Rub=getIntValMTB(tbGet5000Rub);
		//������������� ����� � ����� ��������� �������� � ��������
		int reqExchangeSum=numGet10Rub*10+numGet50Rub*50+numGet100Rub*100+numGet500Rub*500+numGet1000Rub*1000+numGet5000Rub*5000;
		//���������� ����� � ��������� �� ���������
		int billsBeforeGet10=MyForm.appInstance.getStateOfATM()[d10].amount;
		int billsBeforeGet50=MyForm.appInstance.getStateOfATM()[d50].amount;
		int billsBeforeGet100=MyForm.appInstance.getStateOfATM()[d100].amount;
		int billsBeforeGet500=MyForm.appInstance.getStateOfATM()[d500].amount;
		int billsBeforeGet1000=MyForm.appInstance.getStateOfATM()[d1000].amount;
		int billsBeforeGet5000=MyForm.appInstance.getStateOfATM()[d5000].amount;		
		//�������������� ���������� ����� � ��������� ����� ���������
		int billsAfterGet10=billsBeforeGet10-numGet10Rub;
		int billsAfterGet50=billsBeforeGet50-numGet50Rub;
		int billsAfterGet100=billsBeforeGet100-numGet100Rub;
		int billsAfterGet500=billsBeforeGet500-numGet500Rub;
		int billsAfterGet1000=billsBeforeGet1000-numGet1000Rub;
		int billsAfterGet5000=billsBeforeGet5000-numGet5000Rub;
		
		//���� �� ��������� �� ���� ���� ��� ������� ����
		if(reqExchangeSum==0)
		{
			return;
		}
		//���� ������������� ����� � ����� ��������� �������� � �������� �� ������������� ������������� ����� � ������� ����� ��������� ��������
		if(reqExchangeSum!=reqSum)
		{
			MessageBox.Show("��������� ���������� ����� �� ������������� ������������� �����.\n������� ������ ����������","��������� �������� ������� � ��������");
			return;
		}
		//���� ����� ������-���� ����������� � ��������� ������������
		if(billsAfterGet10<0 ||
			billsAfterGet50<0 ||
			billsAfterGet100<0 ||
			billsAfterGet500<0 ||
			billsAfterGet1000<0 ||
			billsAfterGet5000<0)
		{
			MessageBox.Show("��������� ���������� ����� ��������� ���������.\n������� ������ ����������","��������� �������� ������� � ��������");
			return;
		}
		
		//������� 10 �������� ����� �� ���������
		MyForm.appInstance.setBillsInATM(d10, billsAfterGet10);
		//������� 50 �������� ����� �� ���������
		MyForm.appInstance.setBillsInATM(d50, billsAfterGet50);
		//������� 100 �������� ����� �� ���������
		MyForm.appInstance.setBillsInATM(d100, billsAfterGet100);
		//������� 500 �������� ����� �� ���������
		MyForm.appInstance.setBillsInATM(d500, billsAfterGet500);
		//������� 1000 �������� ����� �� ���������
		MyForm.appInstance.setBillsInATM(d1000, billsAfterGet1000);
		//������� 5000 �������� ����� �� ���������
		MyForm.appInstance.setBillsInATM(d5000, billsAfterGet5000);
		
		//��������� ������� � ��������� ������� � ���������� � �������� ���������� �����
		string strNumGet10Rub=(numGet10Rub>0)?("\n"+numGet10Rub.ToString()+" - 10 �������� �����"):"";
		string strNumGet50Rub=(numGet50Rub>0)?("\n"+numGet50Rub.ToString()+" - 50 �������� �����"):"";
		string strNumGet100Rub=(numGet100Rub>0)?("\n"+numGet100Rub.ToString()+" - 100 �������� �����"):"";
		string strNumGet500Rub=(numGet500Rub>0)?("\n"+numGet500Rub.ToString()+" - 500 �������� �����"):"";
		string strNumGet1000Rub=(numGet1000Rub>0)?("\n"+numGet1000Rub.ToString()+" - 1000 �������� �����"):"";
		string strNumGet5000Rub=(numGet5000Rub>0)?("\n"+numGet5000Rub.ToString()+" - 5000 �������� �����"):"";
		
		MessageBox.Show("�������� "+(reqExchangeSum).ToString()+" ���.:"+strNumGet10Rub+strNumGet50Rub+strNumGet100Rub+strNumGet500Rub+strNumGet1000Rub+strNumGet5000Rub,
		"��������� �������� ������� � ��������");
		
		this.Hide();
	}
}

//����� ���������� ���� ����� ����� �����
class myTextBox : TextBox
{
	//���������� ������� ��������� ������� ��������� ������ � TextBox
	protected override void OnTextChanged(EventArgs e)
	{
		string s=getMyTBStr();
		for(int i=0; i<s.Length; i++)
		{
			if(s[i]<'0' || s[i]>'9')
			{
				MessageBox.Show("������� ��������������� ����� �����","����������� ���������� ���� � ������");
				this.Text="";
			}
		}		
	}

	//�������� ����� � ���� ����� ������������ ���������� ���� � ������
	public string getMyTBStr()
	{
		return this.Text;
	}	
}