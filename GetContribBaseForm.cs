using System;
using System.Drawing;
using System.Windows.Forms;

//Базовый класс для GetCashExchangeForm и ContribForm
class GetContribBaseForm : Form
{
    //Массив текстовых элементов ввода количества купюр для получения/внесения из/в банкомат(а)
    protected NonNegativeIntegersTextBox[] tbGetContrib = new NonNegativeIntegersTextBox[ConstantsATM.NumOfDignities];

    //Метка - приглашение
    protected Label labelInvitation;

    //Кнопка получения/внесения средств
    protected Button bGetContrib;

    public GetContribBaseForm()
    {
        Text = "Внесение наличных средств в банкомат";//Заголовок окна внесения средств

        //Метка - приглашение
        labelInvitation = new Label();
        labelInvitation.Location = new Point(15, 15);
        labelInvitation.AutoSize = true;
        labelInvitation.TextAlign = ContentAlignment.BottomLeft;
        this.Controls.Add(labelInvitation);

        //Массив меток для полей внесения количества купюр
        Label[] labelGetContrib = new Label[ConstantsATM.NumOfDignities];
       //Вертикальная координата строки с текущим номиналом купюр в форме
        int Y=labelInvitation.Height + 25;
        for(int i=0; i<ConstantsATM.NumOfDignities; i++)
        {
            //Метка для поля внесения количества купюр i-го номинала
            labelGetContrib[i] = new Label();
            labelGetContrib[i].Text = ConstantsATM.ValuesOfDignities[i].ToString() + " руб.:";
            Y += (i > 0) ? labelGetContrib[i - 1].Height + 2 : 0;
            labelGetContrib[i].Location = new Point(15, Y);
            labelGetContrib[i].AutoSize = true;
            labelGetContrib[i].TextAlign = ContentAlignment.BottomLeft;
            this.Controls.Add(labelGetContrib[i]);

            //Текстбокс (поле) внесения количества купюр i-го номинала
            tbGetContrib[i] = new NonNegativeIntegersTextBox();
            tbGetContrib[i].Text = "";
            tbGetContrib[i].Location = new Point(labelGetContrib[i].Width + 17, Y);
            tbGetContrib[i].Width = 30;
            tbGetContrib[i].Height = labelGetContrib[i].Height;
            tbGetContrib[i].TextAlign = HorizontalAlignment.Right;
            this.Controls.Add(tbGetContrib[i]);
        }

        //Кнопка внесения средств
        bGetContrib = new Button();
        Y += labelGetContrib[ConstantsATM.NumOfDignities - 1].Height + 10;
        bGetContrib.Location = new Point(15, Y);
        bGetContrib.AutoSize = true;
        bGetContrib.TextAlign = ContentAlignment.MiddleCenter;
        this.Controls.Add(bGetContrib);
    }
}
