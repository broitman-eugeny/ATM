using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

//Главная форма получения наличных
class GetCashForm : Form
{
    //Элемент ввода запрашиваемой суммы
    public static NonNegativeIntegersTextBox tbGetCash;
    //Элементы выбора крупных/мелких купюр
    private RadioButton rbLargeBills;//крупные купюры
    private RadioButton rbSmallBills;//мелкие купюры

    public GetCashForm()
    {
        Text = "Получение наличных средств";//Заголовок окна получения средств

        //Метка - приглашение
        Label labelInvitation = new Label();
        labelInvitation.Text = "Укажите требуемую сумму в рублях:";
        labelInvitation.Location = new Point(15, 15);
        labelInvitation.AutoSize = true;
        labelInvitation.TextAlign = ContentAlignment.BottomLeft;
        this.Controls.Add(labelInvitation);

        //Textbox для ввода запрашиваемой суммы
        tbGetCash = new NonNegativeIntegersTextBox();
        tbGetCash.Text = "";
        tbGetCash.Location = new Point(labelInvitation.Width + 17, 15);
        tbGetCash.Width = 50;
        tbGetCash.Height = labelInvitation.Height;
        tbGetCash.TextAlign = HorizontalAlignment.Right;
        this.Controls.Add(tbGetCash);

        //GroupBox для выбора крупных/мелких купюр
        GroupBox gbLargeSmallBills = new GroupBox();
        rbLargeBills = new RadioButton();
        rbSmallBills = new RadioButton();
        gbLargeSmallBills.Controls.Add(rbLargeBills);
        gbLargeSmallBills.Controls.Add(rbSmallBills);
        gbLargeSmallBills.Location = new Point(15, tbGetCash.Height + 17);
        gbLargeSmallBills.Width = rbLargeBills.Width + rbSmallBills.Width + 32;
        gbLargeSmallBills.Height = rbLargeBills.Height + 30;
        gbLargeSmallBills.Text = "Какими купюрами выдать сумму?";
        rbLargeBills.Text = "Крупными";
        rbSmallBills.Text = "С разменом";
        rbLargeBills.Location = new Point(15, 20);
        rbSmallBills.Location = new Point(rbLargeBills.Width + 17, 20);
        rbLargeBills.AutoSize = true;
        rbSmallBills.AutoSize = true;
        Controls.Add(gbLargeSmallBills);
        rbLargeBills.Checked = true;

        //Кнопка получения наличных
        Button bGet = new Button();
        bGet.Text = "Получить";
        bGet.Location = new Point(15, gbLargeSmallBills.Height + tbGetCash.Height + 19);
        bGet.AutoSize = true;
        bGet.TextAlign = ContentAlignment.MiddleCenter;
        this.Controls.Add(bGet);
        bGet.Click += new System.EventHandler(bGet_Clicked);//Установка обработчика нажатия на кнопку получения наличных
    }

    //Обработчик нажатия на кнопку получения наличных
    protected void bGet_Clicked(object sender, EventArgs e)
    {
        //Количество купюр выдаваемых банкоматом
        int[] numBills = new int[ConstantsATM.NumOfDignities];
        //Количество купюр в банкомате до получения наличных
        int[] billsBeforeGet = DataATM.getBillsFromATM();
        //Требуемая сумма
        int sumReq = tbGetCash.getIntValNNITB();
        //Вычисление количества купюр для выдачи требуемой суммы
        if (sumReq > 0)//Требуемая сумма не нулевая
        {
            if (rbLargeBills.Checked == true)//Выбрано крупными купюрами
            {
                int remains = sumReq;//Остаток запрашиваемой суммы после исключения суммы обеспечиваемой купюрами большего достоинства
                for (int i = ConstantsATM.NumOfDignities-1; i >= 0; i--)//Начиная с крупных купюр
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
                    return;
                }

                //Изъятие купюр из банкомата
                for (int i = 0; i < ConstantsATM.NumOfDignities; i++)
                {
                    DataATM.setBillsInATM(i, billsBeforeGet[i] - numBills[i], FormATM._BillsChangedCallback);
                }

                //Строковая вставка в сообщение клиенту о количестве и номинале получаемых купюр
                StringBuilder strNumGet = new StringBuilder();
                for (int i = 0; i < ConstantsATM.NumOfDignities; i++)
                {
                    strNumGet.Append((numBills[i] > 0) ? ("\n" + numBills[i].ToString() + " - " + ConstantsATM.ValuesOfDignities[i] + " рублевых купюр") : "");
                }

                MessageBox.Show("Получите " + sumReq.ToString() + " руб.:" + strNumGet,
                "Получение наличных средств крупными купюрами");
            }
            else//Выбрано купюрами c разменом
            {
                GetCashExchangeForm gcEForm = new GetCashExchangeForm();
                gcEForm.ShowDialog(this);
                gcEForm.Dispose();
            }
            this.Hide();
        }
    }
}