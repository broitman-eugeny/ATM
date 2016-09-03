using System;
using System.Windows.Forms;

//Форма внесения средств
class ContribForm : GetContribBaseForm
{
    public ContribForm() : base()//сначала выполнить конструктор базового класса
    {
        Text = "Внесение наличных средств в банкомат";//Заголовок окна внесения средств

        //Метка - приглашение, унаследованная от базового класса
        labelInvitation.Text = "Укажите количество вносимых купюр достоинством:";

        //Кнопка внесения средств, унаследованная от базового класса
        bGetContrib.Text = "Внести";
        bGetContrib.Click += new System.EventHandler(bContrib_Clicked);//Установка обработчика нажатия на кнопку внесения средств
    }

    //Обработчик нажатия на кнопку внесения средств
    protected void bContrib_Clicked(object sender, EventArgs e)
    {
        bool success;//внесены все купюры
        int[] Bills=new int[ConstantsATM.NumOfDignities];//количество вносимых купюр, введенных пользователем

        for (int i = 0; i < ConstantsATM.NumOfDignities; i++)
        {
            Bills[i] = tbGetContrib[i].getIntValNNITB();
        }

        //Внесение купюр в банкомат
        success=GetContribManager.Contribute(Bills, FormATM._BillsChangedCallback);

        if(success)
        {
            this.Hide();
        }
        else
        {
            for (int i = 0; i < ConstantsATM.NumOfDignities; i++)
            {
                tbGetContrib[i].Text = "";
            }
        }
    }
}