using System;
using System.Text;
using System.Windows.Forms;

class GetContribManager
{
    //Функция внесения средств в банкомат 
    //Возвращает true при успешном внесении всех купюр, false - если по какой-либо причине внесены не все купюры,
    //или не внесено ни одной купюры
    public static bool Contribute(int[] Bills, DataATM.BillsChanged BillsChangedCallback)
	{
        //Количество купюр в банкомате до пополнения
        int[] billsBeforeContrib = DataATM.getBillsFromATM();
        //Предполагаемое количество купюр в банкомате после пополнения
        int[] billsAfterContrib = new int[ConstantsATM.NumOfDignities];
        for (int i = 0; i < ConstantsATM.NumOfDignities; i++ )
        {
            billsAfterContrib[i] = billsBeforeContrib[i] + Bills[i];
        }

        bool ExcessFlag = false;//Признак превышения максимального количества купюр какого либо номинала
        bool ContribFlag = false;//Признак внесения в банкомат купюр какого либо номинала
        //Внесение купюр соответствующего достоинства в банкомат (в массив stateOfATM в классе DataATM)
        for (int i = 0; i < ConstantsATM.NumOfDignities; i++)
        {
            DataATM.setBillsInATM(i, billsAfterContrib[i], BillsChangedCallback);
            if (billsAfterContrib[i] > ConstantsATM.MaxBillsInATM[i])//Превышение максимального количества купюр i-го номинала
            {
                ExcessFlag = true;
            }
        }

        //Фактическое количество купюр в банкомате после пополнения
        billsAfterContrib = DataATM.getBillsFromATM();

        for (int i = 0; i < ConstantsATM.NumOfDignities; i++)
        {
            //Успешное внесение в банкомат купюр хотя бы номинала
            if(billsAfterContrib[i] > billsBeforeContrib[i])
            {
                ContribFlag = true;
                break;
            }
        }

        //Если количество каких либо из купюр превышает максимально допустимое в банкомате
        if (ExcessFlag == true)
        {
            if (ContribFlag==true)//Какие либо из купюр внесены в банкомат
            {
                MessageBox.Show("Остальные купюры внесены успешно", "Внесение средств");
            }
        }
        else
        {
            if (ContribFlag == true)//Какие либо из купюр внесены в банкомат
            {
                MessageBox.Show("Купюры внесены успешно", "Внесение средств");
                return true;
            }
        }

        return false;
    }

    //Функция получения наличных средств с разменом
    //Возвращает true при успешном получении всей суммы, false - если по какой-либо причине деньги не получены
    public static bool GetCashExchange(int reqSum, int[] numGet, DataATM.BillsChanged BillsChangedCallback)
    {
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
            reqExchangeSum += numGet[i] * ConstantsATM.ValuesOfDignities[i];
            billsAfterGet[i] = billsBeforeGet[i] - numGet[i];
            if (billsAfterGet[i] < 0)//Если купюр какого-либо достоинства в банкомате недостаточно
            {
                ExcessFlag = true;
            }
        }

        //Если не заполнено ни одно поле или введены нули
        if (reqExchangeSum == 0)
        {
            return false;
        }
        //Если запрашиваемая сумма в форме получения наличных с разменом не соответствует запрашиваемой сумме в главной форме получения наличных
        if (reqExchangeSum != reqSum)
        {
            MessageBox.Show("Введенное количество купюр не соответствует запрашиваемой сумме.\nВведите другое количество", "Получение наличных средств с разменом");
            return false;
        }
        //Если купюр какого-либо достоинства в банкомате недостаточно
        if (ExcessFlag == true)
        {
            MessageBox.Show("Введенное количество купюр превышает имеющееся.\nВведите другое количество", "Получение наличных средств с разменом");
            return false;
        }

        //Изъятие купюр из банкомата
        for (int i = 0; i < ConstantsATM.NumOfDignities; i++)
        {
            DataATM.setBillsInATM(i, billsAfterGet[i], BillsChangedCallback);
        }

        //Строковая вставка в сообщение клиенту о количестве и номинале получаемых купюр
        StringBuilder strNumGet = new StringBuilder();
        for (int i = 0; i < ConstantsATM.NumOfDignities; i++)
        {
            strNumGet.Append((numGet[i] > 0) ? ("\n" + numGet[i].ToString() + " - " + ConstantsATM.ValuesOfDignities[i] + " рублевых купюр") : "");
        }

        MessageBox.Show("Получите " + reqSum.ToString() + " руб.:" + strNumGet,
        "Получение наличных средств с разменом");

        return true;
    }

    //Функция получения наличных средств крупными купюрами
    //Возвращает true при успешном получении всей суммы, false - если по какой-либо причине деньги не получены
    public static bool GetLargeCash(int sumReq, DataATM.BillsChanged BillsChangedCallback)
    {
        //Количество купюр выдаваемых банкоматом
        int[] numBills = new int[ConstantsATM.NumOfDignities];
        //Количество купюр в банкомате до получения наличных
        int[] billsBeforeGet = DataATM.getBillsFromATM();
        int remains = sumReq;//Остаток запрашиваемой суммы после исключения суммы обеспечиваемой купюрами большего достоинства
        for (int i = ConstantsATM.NumOfDignities - 1; i >= 0; i--)//Начиная с крупных купюр
        {
            numBills[i] = remains / ConstantsATM.ValuesOfDignities[i];//Количество купюр i-го достоинства, требуемых для запрашиваемой суммы
            if (billsBeforeGet[i] < numBills[i])//Если в банкомате нет столько 5000 купюр
            {
                numBills[i] = billsBeforeGet[i];//Забираем все из банкомата
            }
            remains -= (ConstantsATM.ValuesOfDignities[i] * numBills[i]);
        }
        if (remains > 0)//Сумма не кратна 10
        {
            if (remains < 10)//Сумма не кратна 10
            {
                MessageBox.Show("Отсутствуют купюры достоинством меньше 10 руб.\nВведите значение суммы кратное 10", "Получение наличных средств");
            }
            else//Не хватает купюр требуемого достоинства
            {
                MessageBox.Show("В банкомате отсутствует требуемая сумма либо купюры требуемого достоинства.\nВведите другое значение суммы", "Получение наличных средств");
            }
            return false;
        }

        //Изъятие купюр из банкомата
        for (int i = 0; i < ConstantsATM.NumOfDignities; i++)
        {
            DataATM.setBillsInATM(i, billsBeforeGet[i] - numBills[i], BillsChangedCallback);
        }

        //Строковая вставка в сообщение клиенту о количестве и номинале получаемых купюр
        StringBuilder strNumGet = new StringBuilder();
        for (int i = 0; i < ConstantsATM.NumOfDignities; i++)
        {
            strNumGet.Append((numBills[i] > 0) ? ("\n" + numBills[i].ToString() + " - " + ConstantsATM.ValuesOfDignities[i] + " рублевых купюр") : "");
        }

        MessageBox.Show("Получите " + sumReq.ToString() + " руб.:" + strNumGet,
        "Получение наличных средств крупными купюрами");
        return true;
    }
}
