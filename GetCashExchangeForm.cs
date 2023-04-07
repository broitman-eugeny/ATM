using System;
using System.Text;
using System.Windows.Forms;

//Форма получения наличных с разменом
class GetCashExchangeForm : GetContribBaseForm
{
    public GetCashExchangeForm() : base()//сначала выполнить конструктор базового класса
    {
        Text = "Получение наличных средств с разменом";//Заголовок окна получения наличных с разменом

        //Метка - приглашение, унаследованная от базового класса
        labelInvitation.Text = "Укажите количество купюр достоинством:";

        //Кнопка получения средств, унаследованная от базового класса
        bGetContrib.Text = "Получить";
        bGetContrib.Click += new System.EventHandler(bGet_Clicked);//Установка обработчика нажатия на кнопку получения средств
    }

    //Обработчик нажатия на кнопку получения средств
    protected void bGet_Clicked(object sender, EventArgs e)
    {
        bool success;//сумма получена (выдана банкоматом)
        //Запрашиваемая сумма в главной форме получения наличных
        int reqSum = GetCashForm.tbGetCash.getIntValNNITB();
        //Количество запрашиваемых купюр
        int[] numGet = new int[ConstantsATM.NumOfDignities];
        
        for (int i = 0; i < ConstantsATM.NumOfDignities; i++)
        {
            numGet[i] = tbGetContrib[i].getIntValNNITB();
        }
        
        //Выдача купюр из банкомата
        success = GetContribManager.GetCashExchange(reqSum, numGet, FormATM._BillsChangedCallback);

        if (success)
        {
            this.Hide();
        }        
    }
}
