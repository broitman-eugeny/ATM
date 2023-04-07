using System;
using System.Drawing;
using System.Windows.Forms;

//Форма приложения
class FormATM : Form
{
    //Метки состояния банкомата по достоинствам купюр
    static Label[] LblDigBills = new Label[ConstantsATM.NumOfDignities];

    //Инициализация делегата функцией отображения количества купюр на главной форме после его изменения в масссиве stateOfATM функцией setBillsInATM в классе DataATM
    public static DataATM.BillsChanged _BillsChangedCallback = new DataATM.BillsChanged(OnBillsChangedCallback);
    
    public FormATM()
    {
        this.Text = "Банкомат";//Заголовок окна программы
        this.Width = 250;//Ширина окна
        this.Height = 250;//Высота окна

        //Меню пограммы
        MainMenu mnuFileMenu = new MainMenu();
        this.Menu = mnuFileMenu;
        MenuItem MenuItemFile = new MenuItem("&File");
        MenuItemFile.MenuItems.Add("E&xit", new System.EventHandler(this.MenuExit_Click));
        MenuItem MenuItemOperations = new MenuItem("&Operations");
        MenuItemOperations.MenuItems.Add("&Contribute to ATM", new System.EventHandler(this.MenuContribute_Click));
        MenuItemOperations.MenuItems.Add("&Get cash from ATM", new System.EventHandler(this.MenuGet_Click));
        mnuFileMenu.MenuItems.Add(MenuItemFile);
        mnuFileMenu.MenuItems.Add(MenuItemOperations);

        //Инициализация меток состояния банкомата (количество имеющихся купюр, максимальное количество)
        //Общая надпись
        Label labelInATM = new Label();
        labelInATM.Text = "Наличие купюр в банкомате:";
        labelInATM.Location = new Point(15, 15);
        labelInATM.AutoSize = true;
        labelInATM.TextAlign = ContentAlignment.BottomLeft;
        this.Controls.Add(labelInATM);
        int vShift = 10;//Дополнительный вертикальный сдвиг между метками
        for (int i = 0; i < ConstantsATM.NumOfDignities; i++)
        {
            LblDigBills[i]=new Label();
            if (i > 0)//Верхняя метка по достоинству купюр
            {
                vShift += LblDigBills[i-1].Height + 2;
            }
            LblDigBills[i].Location = new Point(15, labelInATM.Location.Y + labelInATM.Height + vShift);
            LblDigBills[i].AutoSize = true;
            LblDigBills[i].TextAlign = ContentAlignment.BottomLeft;
            LblDigBills[i].Text = ConstantsATM.ValuesOfDignities[i].ToString() + " руб.: " + "0 шт. из макс.=" + ConstantsATM.MaxBillsInATM[i].ToString() + " шт.";
            this.Controls.Add(LblDigBills[i]);
        }        
    }

    //Изменение метки на главной форме (состояние банкомата) при изменении количества купюр в классе DataATM достоинством с порядковым номером digIndex
    public static void OnBillsChangedCallback(int digIndex)
    {
        LblDigBills[digIndex].Text = ConstantsATM.ValuesOfDignities[digIndex].ToString() + " руб.: " + DataATM.getBillsFromATM()[digIndex].ToString() + " шт. из макс.=" + ConstantsATM.MaxBillsInATM[digIndex].ToString() + " шт.";
    }
    //При выборе меню File\Exit
    private void MenuExit_Click(Object sender, EventArgs e)
    {
        Application.Exit();
    }

    //При выборе меню Operations\Contribute to ATM
    public void MenuContribute_Click(Object sender, EventArgs e)
    {
        ContribForm cForm = new ContribForm();
        cForm.ShowDialog(this);
        cForm.Dispose();
    }

    //При выборе меню Operations\Get cash from ATM
    private void MenuGet_Click(Object sender, EventArgs e)
    {
        //Экземпляр класса главной формы получения наличных
        GetCashForm gcForm = new GetCashForm();
        gcForm.ShowDialog(this);
        gcForm.Dispose();
    }
}
