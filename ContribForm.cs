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
        //Количество купюр в банкомате до пополнения
        int[] billsBeforeContrib = DataATM.getBillsFromATM();
        //Предполагаемое количество купюр в банкомате после пополнения
        int[] billsAfterContrib = new int[ConstantsATM.NumOfDignities];
        for (int i = 0; i < ConstantsATM.NumOfDignities; i++ )
        {
            billsAfterContrib[i] = billsBeforeContrib[i] + tbGetContrib[i].getIntValNNITB();
        }

        bool ExcessFlag = false;//Признак превышения максимального количества купюр какого либо номинала
        bool ContribFlag = false;//Признак внесения в банкомат купюр какого либо номинала
        //Внесение купюр соответствующего достоинства в банкомат (в массив stateOfATM в классе DataATM)
        for (int i = 0; i < ConstantsATM.NumOfDignities; i++)
        {
            DataATM.setBillsInATM(i, billsAfterContrib[i], FormATM._BillsChangedCallback);
            if (billsAfterContrib[i] > ConstantsATM.MaxBillsInATM[i])//Превышение максимального количества купюр i-го номинала
            {
                ExcessFlag = true;
            }
        }

        //Фактическое количество купюр в банкомате после пополнения
        billsAfterContrib = DataATM.getBillsFromATM();

        for (int i = 0; i < ConstantsATM.NumOfDignities; i++)
        {
            //Успешное внесение в банкомат купюр какого либо номинала
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
                this.Hide();
            }
        }
        for (int i = 0; i < ConstantsATM.NumOfDignities; i++)
        {
            tbGetContrib[i].Text = "";
        }
    }
}