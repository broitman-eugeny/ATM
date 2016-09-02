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
        //Запрашиваемая сумма в главной форме получения наличных
        int reqSum = GetCashForm.tbGetCash.getIntValNNITB();
        //Количество запрашиваемых купюр
        int[] numGet = new int[ConstantsATM.NumOfDignities];
        //Запрашиваемая сумма в форме получения наличных с разменом
        int reqExchangeSum = 0;
        //Количество купюр в банкомате до получения
        int[] billsBeforeGet = DataATM.getBillsFromATM();
        //Предполагаемое количество купюр в банкомате после получения
        int[] billsAfterGet = new int[ConstantsATM.NumOfDignities];
        //Признак превышения максимального количества купюр какого либо номинала
        bool ExcessFlag = false;
        for (int i = 0; i < ConstantsATM.NumOfDignities; i++)
        {
            numGet[i] = tbGetContrib[i].getIntValNNITB();
            reqExchangeSum+=numGet[i]*ConstantsATM.ValuesOfDignities[i];
            billsAfterGet[i] = billsBeforeGet[i] - numGet[i];
            if(billsAfterGet[i]<0)//Если купюр какого-либо достоинства в банкомате недостаточно
            {
                ExcessFlag = true;
            }
        }

        //Если не заполнено ни одно поле или введены нули
        if (reqExchangeSum == 0)
        {
            return;
        }
        //Если запрашиваемая сумма в форме получения наличных с разменом не соответствует запрашиваемой сумме в главной форме получения наличных
        if (reqExchangeSum != reqSum)
        {
            MessageBox.Show("Введенное количество купюр не соответствует запрашиваемой сумме.\nВведите другое количество", "Получение наличных средств с разменом");
            return;
        }
        //Если купюр какого-либо достоинства в банкомате недостаточно
        if (ExcessFlag==true)
        {
            MessageBox.Show("Введенное количество купюр превышает имеющееся.\nВведите другое количество", "Получение наличных средств с разменом");
            return;
        }

        //Изъятие купюр из банкомата
        for (int i = 0; i < ConstantsATM.NumOfDignities; i++)
        {
            DataATM.setBillsInATM(i, billsAfterGet[i], FormATM._BillsChangedCallback);
        }

        //Строковая вставка в сообщение клиенту о количестве и номинале получаемых купюр
        StringBuilder strNumGet = new StringBuilder();
        for (int i = 0; i < ConstantsATM.NumOfDignities; i++)
        {
            strNumGet.Append((numGet[i] > 0) ? ("\n" + numGet[i].ToString() + " - " + ConstantsATM.ValuesOfDignities[i] + " рублевых купюр") : "");
        }

        MessageBox.Show("Получите " + reqSum.ToString() + " руб.:" + strNumGet,
        "Получение наличных средств с разменом");

        this.Hide();
    }
}