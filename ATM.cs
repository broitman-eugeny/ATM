//Тестовое задание «Разработчик C#»
//Задание №2
//Программа Банкомат

//Пространства имен
using System;//Здесь содержится набор основных классов, и мы здесь создаем свой класс
using System.Windows.Forms;//Для работы с формами
using System.Drawing;//Доступ к основным функциям GDI
//using System.IO;//Системный ввод вывод (работа с файлами)
//using System.Text;//Для Encoding
//using System.Windows.Controls;//Для TextChangedEventArgs

class MyForm : AppForm 
{
	public static MyForm appInstance;
		
	// Satisfies rule: MarkWindowsFormsEntryPointsWithStaThread.
	[STAThread]//Однопоточное приложение (для возможности использования компонентов Windows Forms)
	public static void Main()
	{
		appInstance=new MyForm();
		Application.Run(appInstance);
	}
}

//Структура состояния банкомата для одного достоинства купюр
struct inATM
{
	public int amount;//Количество купюр данного достоинства, имеющихся в банкомате
	public int maxAmount;//Максимально возможное количество купюр данного достоинства, помещаемое в банкомат
	public Label lbl;//Метка количества купюр, имеющихся в банкомате	
}

//Форма приложения
class AppForm : Form
{
	//Индексы элементов в массиве stateOfATM в соответствии с достоинством купюр
	const int d10=0;
	const int d50=1;
	const int d100=2;
	const int d500=3;
	const int d1000=4;
	const int d5000=5;
	//Массив состояния банкомата (количество и достоинство купюр)
	private inATM[] stateOfATM = new inATM[6];
	
	//Экземпляр класса главной формы получения наличных
	public static GetCashForm gcForm;
	
	//Функция получения состояния банкомата
	public inATM[] getStateOfATM()
	{
		return this.stateOfATM;
	}
	
	//Функция установки количества купюр заданного достоинства в банкомате и соответствующей метки состояния банкомата
	//digIndex - индекс достоинства купюр
	//amount - количество купюр данного достоинства
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
			MessageBox.Show("Число купюр превышает максимально допустимое","Купюры "+s+" руб.");
			return;
		}
		this.stateOfATM[digIndex].amount=amount;		
		this.stateOfATM[digIndex].lbl.Text=s+" руб.: "+amount.ToString()+" шт. из макс.="+this.stateOfATM[digIndex].maxAmount.ToString()+" шт.";
	}

	//Функция установки максимально возможного количества купюр данного достоинства, помещаемого в банкомат
	//digIndex - индекс достоинства купюр в соответствии с enum digIndexes
	//maxAmount - максимально возможное количество купюр данного достоинства
	public void setMaxBillsInATM(int digIndex, int maxAmount)
	{
		this.stateOfATM[digIndex].maxAmount=maxAmount;		
	}
	
	public AppForm()
	{
		this.Text="Банкомат";//Заголовок окна программы
		this.Width=250;//Ширина окна
		this.Height=250;//Высота окна

		//Меню пограммы
		MainMenu mnuFileMenu = new MainMenu();
		this.Menu = mnuFileMenu;
		MenuItem MenuItemFile = new MenuItem("&File");
		MenuItemFile.MenuItems.Add("E&xit", new System.EventHandler(this.MenuExit_Click));
		MenuItem MenuItemOperations = new MenuItem("&Operations");
		MenuItemOperations.MenuItems.Add("&Contribute to ATM",new System.EventHandler(this.MenuContribute_Click));
		MenuItemOperations.MenuItems.Add("&Get cash from ATM",new System.EventHandler(this.MenuGet_Click));
		mnuFileMenu.MenuItems.Add(MenuItemFile);
		mnuFileMenu.MenuItems.Add(MenuItemOperations);

		//Инициализация меток состояния банкомата (количество имеющихся купюр, максимальное количество)
		//Общая надпись
		Label labelInATM = new Label();
		labelInATM.Text = "Наличие купюр в банкомате:";
		labelInATM.Location = new Point(15,15);
		labelInATM.AutoSize=true;
		labelInATM.TextAlign=ContentAlignment.BottomLeft;
		this.Controls.Add(labelInATM);
		int vShift=10;//Дополнительный вертикальный сдвиг между метками
		//Метки состояния банкомата по достоинствам купюр
		for(int i=d10; i<=d5000; i++)
		{
			setMaxBillsInATM(i, 1000);
			this.stateOfATM[i].lbl=new Label();
			setBillsInATM(i, 0);
			if(i>d10)//Верхняя метка по достоинству купюр
			{
				vShift+=this.stateOfATM[i-1].lbl.Height+2;
			}
			this.stateOfATM[i].lbl.Location = new Point(15,labelInATM.Location.Y+labelInATM.Height+vShift);
			this.stateOfATM[i].lbl.AutoSize=true;
			this.stateOfATM[i].lbl.TextAlign=ContentAlignment.BottomLeft;
			this.Controls.Add(this.stateOfATM[i].lbl);
		}	
	}
	
	//При выборе меню File\Exit
	private void MenuExit_Click(Object sender, EventArgs e)
	{
   		Application.Exit();
	}
	
	//При выборе меню Operations\Contribute to ATM
	public void MenuContribute_Click(Object sender, EventArgs e)
	{
		ContribForm cForm=new ContribForm();
		cForm.ShowDialog(this);
		cForm.Dispose();
	}
	
	//При выборе меню Operations\Get cash from ATM
	private void MenuGet_Click(Object sender, EventArgs e)
	{
		gcForm=new GetCashForm();
		gcForm.ShowDialog(this);
		gcForm.Dispose();
	}
}



//Форма внесения средств
class ContribForm : Form
{
	//Индексы элементов в массиве stateOfATM в соответствии с достоинством купюр
	const int d10=0;
	const int d50=1;
	const int d100=2;
	const int d500=3;
	const int d1000=4;
	const int d5000=5;
	
	//Элементы ввода количества купюр для внесения в банкомат
	private myTextBox tbContrib10Rub;//10 рублевки
	private myTextBox tbContrib50Rub;//50 рублевки
	private myTextBox tbContrib100Rub;//100 рублевки
	private myTextBox tbContrib500Rub;//500 рублевки
	private myTextBox tbContrib1000Rub;//1000 рублевки
	private myTextBox tbContrib5000Rub;//5000 рублевки
	
	public ContribForm()
	{
		Text="Внесение наличных средств в банкомат";//Заголовок окна внесения средств

		//Метка - приглашение
		Label labelInvitation = new Label();
		labelInvitation.Text = "Укажите количество вносимых купюр достоинством:";
		labelInvitation.Location = new Point(15,15);
		labelInvitation.AutoSize=true;
		labelInvitation.TextAlign=ContentAlignment.BottomLeft;
		this.Controls.Add(labelInvitation);

		//Метка для поля внесения 10 рублевых купюр
		Label labelContrib10Rub = new Label();
		labelContrib10Rub.Text = "10 руб.:";
		labelContrib10Rub.Location = new Point(15,labelInvitation.Height+25);
		labelContrib10Rub.AutoSize=true;
		labelContrib10Rub.TextAlign=ContentAlignment.BottomLeft;
		this.Controls.Add(labelContrib10Rub);
		
		//Textbox внесения 10 рублевых купюр
		tbContrib10Rub=new myTextBox();
		tbContrib10Rub.Text = "";
		tbContrib10Rub.Location=new Point(labelContrib10Rub.Width+17,labelInvitation.Height+25);
		tbContrib10Rub.Width=30;
		tbContrib10Rub.Height=labelContrib10Rub.Height;
		tbContrib10Rub.TextAlign=HorizontalAlignment.Right;
		this.Controls.Add(tbContrib10Rub);

		//Метка для поля внесения 50 рублевых купюр
		Label labelContrib50Rub = new Label();
		labelContrib50Rub.Text = "50 руб.:";
		labelContrib50Rub.Location = new Point(15,tbContrib10Rub.Location.Y+tbContrib10Rub.Height+2);
		labelContrib50Rub.AutoSize=true;
		labelContrib50Rub.TextAlign=ContentAlignment.BottomLeft;
		this.Controls.Add(labelContrib50Rub);
		
		//Textbox внесения 50 рублевых купюр
		tbContrib50Rub=new myTextBox();
		tbContrib50Rub.Text = "";
		tbContrib50Rub.Location=new Point(labelContrib50Rub.Width+17,tbContrib10Rub.Location.Y+tbContrib10Rub.Height+2);
		tbContrib50Rub.Width=30;
		tbContrib50Rub.Height=labelContrib50Rub.Height;
		tbContrib50Rub.TextAlign=HorizontalAlignment.Right;
		this.Controls.Add(tbContrib50Rub);
		
		//Метка для поля внесения 100 рублевых купюр
		Label labelContrib100Rub = new Label();
		labelContrib100Rub.Text = "100 руб.:";
		labelContrib100Rub.Location = new Point(15,tbContrib50Rub.Location.Y+tbContrib50Rub.Height+2);
		labelContrib100Rub.AutoSize=true;
		labelContrib100Rub.TextAlign=ContentAlignment.BottomLeft;
		this.Controls.Add(labelContrib100Rub);
		
		//Textbox внесения 100 рублевых купюр
		tbContrib100Rub=new myTextBox();
		tbContrib100Rub.Text = "";
		tbContrib100Rub.Location=new Point(labelContrib100Rub.Width+17,tbContrib50Rub.Location.Y+tbContrib50Rub.Height+2);
		tbContrib100Rub.Width=30;
		tbContrib100Rub.Height=labelContrib100Rub.Height;
		tbContrib100Rub.TextAlign=HorizontalAlignment.Right;
		this.Controls.Add(tbContrib100Rub);
		
		//Метка для поля внесения 500 рублевых купюр
		Label labelContrib500Rub = new Label();
		labelContrib500Rub.Text = "500 руб.:";
		labelContrib500Rub.Location = new Point(15,tbContrib100Rub.Location.Y+tbContrib100Rub.Height+2);
		labelContrib500Rub.AutoSize=true;
		labelContrib500Rub.TextAlign=ContentAlignment.BottomLeft;
		this.Controls.Add(labelContrib500Rub);
		
		//Textbox внесения 500 рублевых купюр
		tbContrib500Rub=new myTextBox();
		tbContrib500Rub.Text = "";
		tbContrib500Rub.Location=new Point(labelContrib500Rub.Width+17,tbContrib100Rub.Location.Y+tbContrib100Rub.Height+2);
		tbContrib500Rub.Width=30;
		tbContrib500Rub.Height=labelContrib500Rub.Height;
		tbContrib500Rub.TextAlign=HorizontalAlignment.Right;
		this.Controls.Add(tbContrib500Rub);
		
		//Метка для поля внесения 1000 рублевых купюр
		Label labelContrib1000Rub = new Label();
		labelContrib1000Rub.Text = "1000 руб.:";
		labelContrib1000Rub.Location = new Point(15,tbContrib500Rub.Location.Y+tbContrib500Rub.Height+2);
		labelContrib1000Rub.AutoSize=true;
		labelContrib1000Rub.TextAlign=ContentAlignment.BottomLeft;
		this.Controls.Add(labelContrib1000Rub);
		
		//Textbox внесения 1000 рублевых купюр
		tbContrib1000Rub=new myTextBox();
		tbContrib1000Rub.Text = "";
		tbContrib1000Rub.Location=new Point(labelContrib1000Rub.Width+17,tbContrib500Rub.Location.Y+tbContrib500Rub.Height+2);
		tbContrib1000Rub.Width=30;
		tbContrib1000Rub.Height=labelContrib1000Rub.Height;
		tbContrib1000Rub.TextAlign=HorizontalAlignment.Right;
		this.Controls.Add(tbContrib1000Rub);
		
		//Метка для поля внесения 5000 рублевых купюр
		Label labelContrib5000Rub = new Label();
		labelContrib5000Rub.Text = "5000 руб.:";
		labelContrib5000Rub.Location = new Point(15,tbContrib1000Rub.Location.Y+tbContrib1000Rub.Height+2);
		labelContrib5000Rub.AutoSize=true;
		labelContrib5000Rub.TextAlign=ContentAlignment.BottomLeft;
		this.Controls.Add(labelContrib5000Rub);
		
		//Textbox внесения 5000 рублевых купюр
		tbContrib5000Rub=new myTextBox();
		tbContrib5000Rub.Text = "";
		tbContrib5000Rub.Location=new Point(labelContrib5000Rub.Width+17,tbContrib1000Rub.Location.Y+tbContrib1000Rub.Height+2);
		tbContrib5000Rub.Width=30;
		tbContrib5000Rub.Height=labelContrib5000Rub.Height;
		tbContrib5000Rub.TextAlign=HorizontalAlignment.Right;
		this.Controls.Add(tbContrib5000Rub);
		
		//Кнопка внесения средств
		Button bContrib=new Button();
		bContrib.Text="Внести";
		bContrib.Location=new Point(15,tbContrib5000Rub.Location.Y+tbContrib1000Rub.Height+10);
		bContrib.AutoSize=true;
		bContrib.TextAlign=ContentAlignment.MiddleCenter;
		this.Controls.Add(bContrib);
		bContrib.Click += new System.EventHandler(bContrib_Clicked);//Установка обработчика нажатия на кнопку внесения средств
	}

	//Получить численное значение myTextBox
	public int getIntValMTB(myTextBox mTB)
	{
		return (mTB.Text=="")?0:Convert.ToInt32(mTB.Text);
	}
	
	//Обработчик нажатия на кнопку внесения средств
	protected void bContrib_Clicked(object sender, EventArgs e)
	{
		//Количество купюр в банкомате до пополнения
		int billsBeforeContrib10=MyForm.appInstance.getStateOfATM()[d10].amount;
		int billsBeforeContrib50=MyForm.appInstance.getStateOfATM()[d50].amount;
		int billsBeforeContrib100=MyForm.appInstance.getStateOfATM()[d100].amount;
		int billsBeforeContrib500=MyForm.appInstance.getStateOfATM()[d500].amount;
		int billsBeforeContrib1000=MyForm.appInstance.getStateOfATM()[d1000].amount;
		int billsBeforeContrib5000=MyForm.appInstance.getStateOfATM()[d5000].amount;		
		//Предполагаемое количество купюр в банкомате после пополнения
		int billsAfterContrib10=billsBeforeContrib10+getIntValMTB(tbContrib10Rub);
		int billsAfterContrib50=billsBeforeContrib50+getIntValMTB(tbContrib50Rub);
		int billsAfterContrib100=billsBeforeContrib100+getIntValMTB(tbContrib100Rub);
		int billsAfterContrib500=billsBeforeContrib500+getIntValMTB(tbContrib500Rub);
		int billsAfterContrib1000=billsBeforeContrib1000+getIntValMTB(tbContrib1000Rub);
		int billsAfterContrib5000=billsBeforeContrib5000+getIntValMTB(tbContrib5000Rub);
		
		//Внесение 10 рублевых купюр
		MyForm.appInstance.setBillsInATM(d10, billsAfterContrib10);
		//Внесение 50 рублевых купюр
		MyForm.appInstance.setBillsInATM(d50, billsAfterContrib50);
		//Внесение 100 рублевых купюр
		MyForm.appInstance.setBillsInATM(d100, billsAfterContrib100);
		//Внесение 500 рублевых купюр
		MyForm.appInstance.setBillsInATM(d500, billsAfterContrib500);
		//Внесение 1000 рублевых купюр
		MyForm.appInstance.setBillsInATM(d1000, billsAfterContrib1000);
		//Внесение 5000 рублевых купюр
		MyForm.appInstance.setBillsInATM(d5000, billsAfterContrib5000);
		if(billsAfterContrib10>MyForm.appInstance.getStateOfATM()[d10].maxAmount ||//Количество каких либо из купюр превышает максимально допустимое в банкомате
			billsAfterContrib50>MyForm.appInstance.getStateOfATM()[d50].maxAmount ||
			billsAfterContrib100>MyForm.appInstance.getStateOfATM()[d100].maxAmount ||
			billsAfterContrib500>MyForm.appInstance.getStateOfATM()[d500].maxAmount ||
			billsAfterContrib1000>MyForm.appInstance.getStateOfATM()[d1000].maxAmount ||
			billsAfterContrib5000>MyForm.appInstance.getStateOfATM()[d5000].maxAmount)
		{
			if(!(MyForm.appInstance.getStateOfATM()[d10].amount==billsBeforeContrib10 &&//Какие либо из купюр внесены в банкомат
			MyForm.appInstance.getStateOfATM()[d50].amount==billsBeforeContrib50 &&
			MyForm.appInstance.getStateOfATM()[d100].amount==billsBeforeContrib100 &&
			MyForm.appInstance.getStateOfATM()[d500].amount==billsBeforeContrib500 &&
			MyForm.appInstance.getStateOfATM()[d1000].amount==billsBeforeContrib1000 &&
			MyForm.appInstance.getStateOfATM()[d5000].amount==billsBeforeContrib5000))
			{
				MessageBox.Show("Остальные купюры внесены успешно", "Внесение средств");
			}			
		}
		else
		{
			if(!(tbContrib10Rub.Text=="" &&//Какие либо из купюр внесены в банкомат
			tbContrib50Rub.Text=="" &&
			tbContrib100Rub.Text=="" &&
			tbContrib500Rub.Text=="" &&
			tbContrib1000Rub.Text=="" &&
			tbContrib5000Rub.Text==""))
			{
				MessageBox.Show("Купюры внесены успешно", "Внесение средств");
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

//Главная форма получения наличных
class GetCashForm : Form
{
	//Индексы элементов в массиве stateOfATM в соответствии с достоинством купюр
	const int d10=0;
	const int d50=1;
	const int d100=2;
	const int d500=3;
	const int d1000=4;
	const int d5000=5;
	
	//Элемент ввода запрашиваемой суммы
	public myTextBox tbGetCash;
	//Элементы выбора крупных/мелких купюр
	private RadioButton rbLargeBills;//крупные купюры
	private RadioButton rbSmallBills;//мелкие купюры
	
	public GetCashForm()
	{
		Text="Получение наличных средств";//Заголовок окна получения средств
		
		//Метка - приглашение
		Label labelInvitation = new Label();
		labelInvitation.Text = "Укажите требуемую сумму в рублях:";
		labelInvitation.Location = new Point(15,15);
		labelInvitation.AutoSize=true;
		labelInvitation.TextAlign=ContentAlignment.BottomLeft;
		this.Controls.Add(labelInvitation);
		
		//Textbox для ввода запрашиваемой суммы
		tbGetCash=new myTextBox();
		tbGetCash.Text = "";
		tbGetCash.Location=new Point(labelInvitation.Width+17,15);
		tbGetCash.Width=50;
		tbGetCash.Height=labelInvitation.Height;
		tbGetCash.TextAlign=HorizontalAlignment.Right;
		this.Controls.Add(tbGetCash);
		
		//GroupBox для выбора крупных/мелких купюр
		GroupBox gbLargeSmallBills = new GroupBox();		
		rbLargeBills = new RadioButton();
		rbSmallBills = new RadioButton();
		gbLargeSmallBills.Controls.Add(rbLargeBills);
		gbLargeSmallBills.Controls.Add(rbSmallBills);
		gbLargeSmallBills.Location = new Point(15, tbGetCash.Height+17);
		gbLargeSmallBills.Width=rbLargeBills.Width+rbSmallBills.Width+32;
		gbLargeSmallBills.Height=rbLargeBills.Height+30;
		gbLargeSmallBills.Text = "Какими купюрами выдать сумму?";
		rbLargeBills.Text = "Крупными";
		rbSmallBills.Text = "С разменом";
		rbLargeBills.Location = new Point(15, 20);
		rbSmallBills.Location = new Point(rbLargeBills.Width+17, 20);
		rbLargeBills.AutoSize=true;
		rbSmallBills.AutoSize=true;
		Controls.Add(gbLargeSmallBills);
		rbLargeBills.Checked=true;
		
		//Кнопка получения наличных
		Button bGet=new Button();
		bGet.Text="Получить";
		bGet.Location=new Point(15,gbLargeSmallBills.Height+tbGetCash.Height+19);
		bGet.AutoSize=true;
		bGet.TextAlign=ContentAlignment.MiddleCenter;
		this.Controls.Add(bGet);
		bGet.Click += new System.EventHandler(bGet_Clicked);//Установка обработчика нажатия на кнопку получения наличных
	}
	
	//Получить численное значение myTextBox
	public int getIntValMTB(myTextBox mTB)
	{
		return (mTB.Text=="")?0:Convert.ToInt32(mTB.Text);
	}
	
	//Обработчик нажатия на кнопку получения наличных
	protected void bGet_Clicked(object sender, EventArgs e)
	{
		//Количество купюр выдаваемых банкоматом
		int numBills5000;
		int numBills1000;
		int numBills500;
		int numBills100;
		int numBills50;
		int numBills10;
		//Количество купюр в банкомате до получения наличных
		int billsBeforeGet10=MyForm.appInstance.getStateOfATM()[d10].amount;
		int billsBeforeGet50=MyForm.appInstance.getStateOfATM()[d50].amount;
		int billsBeforeGet100=MyForm.appInstance.getStateOfATM()[d100].amount;
		int billsBeforeGet500=MyForm.appInstance.getStateOfATM()[d500].amount;
		int billsBeforeGet1000=MyForm.appInstance.getStateOfATM()[d1000].amount;
		int billsBeforeGet5000=MyForm.appInstance.getStateOfATM()[d5000].amount;
		//Требуемая сумма
		int sumReq=getIntValMTB(tbGetCash);		
		//Вычисление количества купюр для выдачи требуемой суммы
		if(sumReq>0)//Требуемая сумма не нулевая
		{
			if(rbLargeBills.Checked==true)//Выбрано крупными купюрами
			{
				numBills5000=sumReq/5000;//Количество 5000 купюр, требуемых для запрашиваемой суммы
				if(billsBeforeGet5000<numBills5000)//Если в банкомате нет столько 5000 купюр
				{
					numBills5000=billsBeforeGet5000;//Забираем все из банкомата
				}
				int remains=sumReq-(5000*numBills5000);//Остаток запрашиваемой суммы после исключения суммы обеспечиваемой 5000 купюрами
				numBills1000=remains/1000;//Количество 1000 купюр, требуемых для остатка запрашиваемой суммы
				if(billsBeforeGet1000<numBills1000)//Если в банкомате нет столько 1000 купюр
				{
					numBills1000=billsBeforeGet1000;//Забираем все из банкомата
				}
				remains-=1000*numBills1000;//Остаток запрашиваемой суммы после исключения суммы обеспечиваемой 1000 купюрами
				numBills500=remains/500;//Количество 500 купюр, требуемых для остатка запрашиваемой суммы
				if(billsBeforeGet500<numBills500)//Если в банкомате нет столько 500 купюр
				{
					numBills500=billsBeforeGet500;//Забираем все из банкомата
				}
				remains-=500*numBills500;//Остаток запрашиваемой суммы после исключения суммы обеспечиваемой 500 купюрами
				numBills100=remains/100;//Количество 100 купюр, требуемых для остатка запрашиваемой суммы
				if(billsBeforeGet100<numBills100)//Если в банкомате нет столько 100 купюр
				{
					numBills100=billsBeforeGet100;//Забираем все из банкомата
				}
				remains-=100*numBills100;//Остаток запрашиваемой суммы после исключения суммы обеспечиваемой 100 купюрами
				numBills50=remains/50;//Количество 50 купюр, требуемых для остатка запрашиваемой суммы
				if(billsBeforeGet50<numBills50)//Если в банкомате нет столько 50 купюр
				{
					numBills50=billsBeforeGet50;//Забираем все из банкомата
				}
				remains-=50*numBills50;//Остаток запрашиваемой суммы после исключения суммы обеспечиваемой 50 купюрами
				numBills10=remains/10;//Количество 10 купюр, требуемых для остатка запрашиваемой суммы
				if(billsBeforeGet10<numBills10)//Если в банкомате нет столько 10 купюр
				{
					MessageBox.Show("В банкомате отсутствует требуемая сумма либо купюры требуемого достоинства.\nВведите другое значение", "Получение наличных средств");
					return;
				}
				remains-=10*numBills10;//Остаток запрашиваемой суммы после исключения суммы обеспечиваемой 10 купюрами
				if(remains>0)//Сумма не кратна 10
				{
					MessageBox.Show("Отсутствуют купюры достоинством меньше 10 руб.\nВведите значение кратное 10", "Получение наличных средств");
					return;
				}
				//Изъятие 10 рублевых купюр из банкомата
				MyForm.appInstance.setBillsInATM(d10, billsBeforeGet10-numBills10);
				//Изъятие 50 рублевых купюр из банкомата
				MyForm.appInstance.setBillsInATM(d50, billsBeforeGet50-numBills50);
				//Изъятие 100 рублевых купюр из банкомата
				MyForm.appInstance.setBillsInATM(d100, billsBeforeGet100-numBills100);
				//Изъятие 500 рублевых купюр из банкомата
				MyForm.appInstance.setBillsInATM(d500, billsBeforeGet500-numBills500);
				//Изъятие 1000 рублевых купюр из банкомата
				MyForm.appInstance.setBillsInATM(d1000, billsBeforeGet1000-numBills1000);
				//Изъятие 5000 рублевых купюр из банкомата
				MyForm.appInstance.setBillsInATM(d5000, billsBeforeGet5000-numBills5000);
		
				//Строковые вставки в сообщение клиенту о количестве и номинале получаемых купюр
				string strNumGet10Rub=(numBills10>0)?("\n"+numBills10.ToString()+" - 10 рублевых купюр"):"";
				string strNumGet50Rub=(numBills50>0)?("\n"+numBills50.ToString()+" - 50 рублевых купюр"):"";
				string strNumGet100Rub=(numBills100>0)?("\n"+numBills100.ToString()+" - 100 рублевых купюр"):"";
				string strNumGet500Rub=(numBills500>0)?("\n"+numBills500.ToString()+" - 500 рублевых купюр"):"";
				string strNumGet1000Rub=(numBills1000>0)?("\n"+numBills1000.ToString()+" - 1000 рублевых купюр"):"";
				string strNumGet5000Rub=(numBills5000>0)?("\n"+numBills5000.ToString()+" - 5000 рублевых купюр"):"";
		
				MessageBox.Show("Получите "+(numBills10*10+numBills50*50+numBills100*100+numBills500*500+numBills1000*1000+numBills5000*5000).ToString()+" руб.:"+strNumGet10Rub+strNumGet50Rub+strNumGet100Rub+strNumGet500Rub+strNumGet1000Rub+strNumGet5000Rub,
				"Получение наличных средств крупными купюрами");				
			}
			else//Выбрано купюрами c разменом
			{
				GetCashExchangeForm gcEForm=new GetCashExchangeForm();
				gcEForm.ShowDialog(this);
				gcEForm.Dispose();
			}
			this.Hide();
		}
	}
}

//Форма получения наличных с разменом
class GetCashExchangeForm : Form
{
	//Индексы элементов в массиве stateOfATM в соответствии с достоинством купюр
	const int d10=0;
	const int d50=1;
	const int d100=2;
	const int d500=3;
	const int d1000=4;
	const int d5000=5;
	
	//Элементы ввода количества купюр для получения наличных
	private myTextBox tbGet10Rub;//10 рублевки
	private myTextBox tbGet50Rub;//50 рублевки
	private myTextBox tbGet100Rub;//100 рублевки
	private myTextBox tbGet500Rub;//500 рублевки
	private myTextBox tbGet1000Rub;//1000 рублевки
	private myTextBox tbGet5000Rub;//5000 рублевки
	
	public GetCashExchangeForm()
	{
		Text="Получение наличных средств с разменом";//Заголовок окна получения наличных с разменом

		//Метка - приглашение
		Label labelInvitation = new Label();
		labelInvitation.Text = "Укажите количество купюр достоинством:";
		labelInvitation.Location = new Point(15,15);
		labelInvitation.AutoSize=true;
		labelInvitation.TextAlign=ContentAlignment.BottomLeft;
		this.Controls.Add(labelInvitation);

		//Метка для поля получения 10 рублевых купюр
		Label labelGet10Rub = new Label();
		labelGet10Rub.Text = "10 руб.:";
		labelGet10Rub.Location = new Point(15,labelInvitation.Height+25);
		labelGet10Rub.AutoSize=true;
		labelGet10Rub.TextAlign=ContentAlignment.BottomLeft;
		this.Controls.Add(labelGet10Rub);
		
		//Textbox получения 10 рублевых купюр
		tbGet10Rub=new myTextBox();
		tbGet10Rub.Text = "";
		tbGet10Rub.Location=new Point(labelGet10Rub.Width+17,labelInvitation.Height+25);
		tbGet10Rub.Width=30;
		tbGet10Rub.Height=labelGet10Rub.Height;
		tbGet10Rub.TextAlign=HorizontalAlignment.Right;
		this.Controls.Add(tbGet10Rub);

		//Метка для поля получения 50 рублевых купюр
		Label labelGet50Rub = new Label();
		labelGet50Rub.Text = "50 руб.:";
		labelGet50Rub.Location = new Point(15,tbGet10Rub.Location.Y+tbGet10Rub.Height+2);
		labelGet50Rub.AutoSize=true;
		labelGet50Rub.TextAlign=ContentAlignment.BottomLeft;
		this.Controls.Add(labelGet50Rub);
		
		//Textbox получения 50 рублевых купюр
		tbGet50Rub=new myTextBox();
		tbGet50Rub.Text = "";
		tbGet50Rub.Location=new Point(labelGet50Rub.Width+17,tbGet10Rub.Location.Y+tbGet10Rub.Height+2);
		tbGet50Rub.Width=30;
		tbGet50Rub.Height=labelGet50Rub.Height;
		tbGet50Rub.TextAlign=HorizontalAlignment.Right;
		this.Controls.Add(tbGet50Rub);
		
		//Метка для поля получения 100 рублевых купюр
		Label labelGet100Rub = new Label();
		labelGet100Rub.Text = "100 руб.:";
		labelGet100Rub.Location = new Point(15,tbGet50Rub.Location.Y+tbGet50Rub.Height+2);
		labelGet100Rub.AutoSize=true;
		labelGet100Rub.TextAlign=ContentAlignment.BottomLeft;
		this.Controls.Add(labelGet100Rub);
		
		//Textbox получения 100 рублевых купюр
		tbGet100Rub=new myTextBox();
		tbGet100Rub.Text = "";
		tbGet100Rub.Location=new Point(labelGet100Rub.Width+17,tbGet50Rub.Location.Y+tbGet50Rub.Height+2);
		tbGet100Rub.Width=30;
		tbGet100Rub.Height=labelGet100Rub.Height;
		tbGet100Rub.TextAlign=HorizontalAlignment.Right;
		this.Controls.Add(tbGet100Rub);
		
		//Метка для поля получения 500 рублевых купюр
		Label labelGet500Rub = new Label();
		labelGet500Rub.Text = "500 руб.:";
		labelGet500Rub.Location = new Point(15,tbGet100Rub.Location.Y+tbGet100Rub.Height+2);
		labelGet500Rub.AutoSize=true;
		labelGet500Rub.TextAlign=ContentAlignment.BottomLeft;
		this.Controls.Add(labelGet500Rub);
		
		//Textbox получения 500 рублевых купюр
		tbGet500Rub=new myTextBox();
		tbGet500Rub.Text = "";
		tbGet500Rub.Location=new Point(labelGet500Rub.Width+17,tbGet100Rub.Location.Y+tbGet100Rub.Height+2);
		tbGet500Rub.Width=30;
		tbGet500Rub.Height=labelGet500Rub.Height;
		tbGet500Rub.TextAlign=HorizontalAlignment.Right;
		this.Controls.Add(tbGet500Rub);
		
		//Метка для поля получения 1000 рублевых купюр
		Label labelGet1000Rub = new Label();
		labelGet1000Rub.Text = "1000 руб.:";
		labelGet1000Rub.Location = new Point(15,tbGet500Rub.Location.Y+tbGet500Rub.Height+2);
		labelGet1000Rub.AutoSize=true;
		labelGet1000Rub.TextAlign=ContentAlignment.BottomLeft;
		this.Controls.Add(labelGet1000Rub);
		
		//Textbox получения 1000 рублевых купюр
		tbGet1000Rub=new myTextBox();
		tbGet1000Rub.Text = "";
		tbGet1000Rub.Location=new Point(labelGet1000Rub.Width+17,tbGet500Rub.Location.Y+tbGet500Rub.Height+2);
		tbGet1000Rub.Width=30;
		tbGet1000Rub.Height=labelGet1000Rub.Height;
		tbGet1000Rub.TextAlign=HorizontalAlignment.Right;
		this.Controls.Add(tbGet1000Rub);
		
		//Метка для поля получения 5000 рублевых купюр
		Label labelGet5000Rub = new Label();
		labelGet5000Rub.Text = "5000 руб.:";
		labelGet5000Rub.Location = new Point(15,tbGet1000Rub.Location.Y+tbGet1000Rub.Height+2);
		labelGet5000Rub.AutoSize=true;
		labelGet5000Rub.TextAlign=ContentAlignment.BottomLeft;
		this.Controls.Add(labelGet5000Rub);
		
		//Textbox получения 5000 рублевых купюр
		tbGet5000Rub=new myTextBox();
		tbGet5000Rub.Text = "";
		tbGet5000Rub.Location=new Point(labelGet5000Rub.Width+17,tbGet1000Rub.Location.Y+tbGet1000Rub.Height+2);
		tbGet5000Rub.Width=30;
		tbGet5000Rub.Height=labelGet5000Rub.Height;
		tbGet5000Rub.TextAlign=HorizontalAlignment.Right;
		this.Controls.Add(tbGet5000Rub);
		
		//Кнопка получения средств
		Button bGet=new Button();
		bGet.Text="Получить";
		bGet.Location=new Point(15,tbGet5000Rub.Location.Y+tbGet1000Rub.Height+10);
		bGet.AutoSize=true;
		bGet.TextAlign=ContentAlignment.MiddleCenter;
		this.Controls.Add(bGet);
		bGet.Click += new System.EventHandler(bGet_Clicked);//Установка обработчика нажатия на кнопку получения средств
	}

	//Получить численное значение myTextBox
	public int getIntValMTB(myTextBox mTB)
	{
		return (mTB.Text=="")?0:Convert.ToInt32(mTB.Text);
	}
	
	//Обработчик нажатия на кнопку получения средств
	protected void bGet_Clicked(object sender, EventArgs e)
	{
		//Запрашиваемая сумма в главной форме получения наличных
		int reqSum=getIntValMTB(AppForm.gcForm.tbGetCash);
		//Количество запрашиваемых купюр
		int numGet10Rub=getIntValMTB(tbGet10Rub);
		int numGet50Rub=getIntValMTB(tbGet50Rub);
		int numGet100Rub=getIntValMTB(tbGet100Rub);
		int numGet500Rub=getIntValMTB(tbGet500Rub);
		int numGet1000Rub=getIntValMTB(tbGet1000Rub);
		int numGet5000Rub=getIntValMTB(tbGet5000Rub);
		//Запрашиваемая сумма в форме получения наличных с разменом
		int reqExchangeSum=numGet10Rub*10+numGet50Rub*50+numGet100Rub*100+numGet500Rub*500+numGet1000Rub*1000+numGet5000Rub*5000;
		//Количество купюр в банкомате до получения
		int billsBeforeGet10=MyForm.appInstance.getStateOfATM()[d10].amount;
		int billsBeforeGet50=MyForm.appInstance.getStateOfATM()[d50].amount;
		int billsBeforeGet100=MyForm.appInstance.getStateOfATM()[d100].amount;
		int billsBeforeGet500=MyForm.appInstance.getStateOfATM()[d500].amount;
		int billsBeforeGet1000=MyForm.appInstance.getStateOfATM()[d1000].amount;
		int billsBeforeGet5000=MyForm.appInstance.getStateOfATM()[d5000].amount;		
		//Предполагаемое количество купюр в банкомате после получения
		int billsAfterGet10=billsBeforeGet10-numGet10Rub;
		int billsAfterGet50=billsBeforeGet50-numGet50Rub;
		int billsAfterGet100=billsBeforeGet100-numGet100Rub;
		int billsAfterGet500=billsBeforeGet500-numGet500Rub;
		int billsAfterGet1000=billsBeforeGet1000-numGet1000Rub;
		int billsAfterGet5000=billsBeforeGet5000-numGet5000Rub;
		
		//Если не заполнено ни одно поле или введены нули
		if(reqExchangeSum==0)
		{
			return;
		}
		//Если запрашиваемая сумма в форме получения наличных с разменом не соответствует запрашиваемой сумме в главной форме получения наличных
		if(reqExchangeSum!=reqSum)
		{
			MessageBox.Show("Введенное количество купюр не соответствует запрашиваемой сумме.\nВведите другое количество","Получение наличных средств с разменом");
			return;
		}
		//Если купюр какого-либо достоинства в банкомате недостаточно
		if(billsAfterGet10<0 ||
			billsAfterGet50<0 ||
			billsAfterGet100<0 ||
			billsAfterGet500<0 ||
			billsAfterGet1000<0 ||
			billsAfterGet5000<0)
		{
			MessageBox.Show("Введенное количество купюр превышает имеющееся.\nВведите другое количество","Получение наличных средств с разменом");
			return;
		}
		
		//Изъятие 10 рублевых купюр из банкомата
		MyForm.appInstance.setBillsInATM(d10, billsAfterGet10);
		//Изъятие 50 рублевых купюр из банкомата
		MyForm.appInstance.setBillsInATM(d50, billsAfterGet50);
		//Изъятие 100 рублевых купюр из банкомата
		MyForm.appInstance.setBillsInATM(d100, billsAfterGet100);
		//Изъятие 500 рублевых купюр из банкомата
		MyForm.appInstance.setBillsInATM(d500, billsAfterGet500);
		//Изъятие 1000 рублевых купюр из банкомата
		MyForm.appInstance.setBillsInATM(d1000, billsAfterGet1000);
		//Изъятие 5000 рублевых купюр из банкомата
		MyForm.appInstance.setBillsInATM(d5000, billsAfterGet5000);
		
		//Строковые вставки в сообщение клиенту о количестве и номинале получаемых купюр
		string strNumGet10Rub=(numGet10Rub>0)?("\n"+numGet10Rub.ToString()+" - 10 рублевых купюр"):"";
		string strNumGet50Rub=(numGet50Rub>0)?("\n"+numGet50Rub.ToString()+" - 50 рублевых купюр"):"";
		string strNumGet100Rub=(numGet100Rub>0)?("\n"+numGet100Rub.ToString()+" - 100 рублевых купюр"):"";
		string strNumGet500Rub=(numGet500Rub>0)?("\n"+numGet500Rub.ToString()+" - 500 рублевых купюр"):"";
		string strNumGet1000Rub=(numGet1000Rub>0)?("\n"+numGet1000Rub.ToString()+" - 1000 рублевых купюр"):"";
		string strNumGet5000Rub=(numGet5000Rub>0)?("\n"+numGet5000Rub.ToString()+" - 5000 рублевых купюр"):"";
		
		MessageBox.Show("Получите "+(reqExchangeSum).ToString()+" руб.:"+strNumGet10Rub+strNumGet50Rub+strNumGet100Rub+strNumGet500Rub+strNumGet1000Rub+strNumGet5000Rub,
		"Получение наличных средств с разменом");
		
		this.Hide();
	}
}

//Класс текстового поля ввода целых чисел
class myTextBox : TextBox
{
	//Перегрузка функции обработки события изменения текста в TextBox
	protected override void OnTextChanged(EventArgs e)
	{
		string s=getMyTBStr();
		for(int i=0; i<s.Length; i++)
		{
			if(s[i]<'0' || s[i]>'9')
			{
				MessageBox.Show("Введите неотрицательное целое число","Минимальное количество букв в словах");
				this.Text="";
			}
		}		
	}

	//Получить текст в поле ввода минимального количества букв в словах
	public string getMyTBStr()
	{
		return this.Text;
	}	
}