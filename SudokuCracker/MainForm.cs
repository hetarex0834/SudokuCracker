namespace SudokuCracker
{
    /// <summary>
    /// �i���v����ǃV�X�e��
    /// </summary>
    public partial class MainForm : Form
    {
        // ���̃}�X�ڂ̌�
        private static readonly int Number = 9;
        // �}�X�ڂ̌�
        private static readonly int Max = Number * Number;
        // �}�X��
        private readonly TextBox[,] txtSquares = new TextBox[Number, Number];
        // �}�X�ڂ̐���
        private readonly int[] sqVals = new int[Number * Number];
        // �}�X�ڂ�X���W�̏����l
        private readonly int StartX = 160;
        // �}�X�ڂ�Y���W�̏����l
        private readonly int StartY = 80;
        // �}�X�ڂ̔z�u�Ԋu
        private readonly int SqSpace = 60;
        // �}�X�ڂ̃T�C�Y
        private readonly int SqSize = 55;

        /// <summary>
        /// �R���|�[�l���g��ݒ�
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            // �}�X�ڂ��쐬
            for (var i = 0; i < Number; i++)
            {
                for (var j = 0; j < Number; j++)
                {
                    txtSquares[i, j] = new TextBox()
                    {
                        ImeMode = ImeMode.Disable,
                        MaxLength = 1,
                        ShortcutsEnabled = false,
                        TabIndex = i * Number + j + 1,
                        Location = new Point(StartX + SqSpace * j, StartY + SqSpace * i),
                        Size = new Size(SqSize, SqSize),
                        Font = new Font("Yu Gothic UI", 18F, FontStyle.Bold, GraphicsUnit.Point),
                        TextAlign = HorizontalAlignment.Center,
                    };

                    // 3�~3�}�X���Ƃɔw�i�F��ݒ�
                    if (i < 3 || 5 < i)
                    {
                        if (3 <= j && j <= 5) txtSquares[i, j].BackColor = Color.LightGray;
                    }
                    else
                    {
                        if (j < 3 || 5 < j) txtSquares[i, j].BackColor = Color.LightGray;
                    }

                    txtSquares[i, j].KeyPress += TxtSquares_KeyPress;
                    Controls.Add(txtSquares[i, j]);
                }
            }
        }

        /// <summary>
        /// ���͉\�ȃL�[�𐧌�
        /// </summary>
        private void TxtSquares_KeyPress(object? sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\b') return;
            if (e.KeyChar < '1' || '9' < e.KeyChar) e.Handled = true;
        }

        /// <summary>
        /// ��ǃ{�^���N���b�N�C�x���g
        /// </summary>
        private void BtnClack_Click(object sender, EventArgs e)
        {
            bool flg = false; // ��Ǌ����t���O
            SetSquareValue();
            Solve(0, ref flg);
        }

        /// <summary>
        /// �}�X�ڂ�������
        /// </summary>
        private void BtnClear_Click(object sender, EventArgs e)
        {
            foreach (var t in txtSquares) t.Text = "";
        }

        /// <summary>
        /// �}�X�ڂ̐�����ݒ�
        /// </summary>
        private void SetSquareValue()
        {
            for (var i = 0; i < Number; i++)
            {
                for (var j = 0; j < Number; j++)
                {
                    sqVals[i * Number + j] = (txtSquares[i, j].Text != "") ? int.Parse(txtSquares[i, j].Text) : 0;
                }
            }
        }

        /// <summary>
        /// �i���v�����
        /// </summary>
        /// <param name="x">�}�X�ڂ̓Y��</param>
        /// <param name="flg">��Ǌ����t���O</param>
        private void Solve(int x, ref bool flg)
        {
            if (x > Max - 1)
            {
                PrintSquare();
                flg = true;
            }
            else
            {
                if (sqVals[x] != 0) Solve(x + 1, ref flg);
                else
                {
                    for (var i = 1; i <= 9; i++)
                    {
                        if (flg) break;
                        if (Check(i, x))
                        {
                            sqVals[x] = i;
                            Solve(x + 1, ref flg);
                            sqVals[x] = 0;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// ��ǌ��ʂ��}�X�ڂɏo��
        /// </summary>
        private void PrintSquare()
        {
            for (var i = 0; i < Number; i++)
            {
                for (var j = 0; j < Number; j++)
                {
                    txtSquares[i, j].Text = $"{sqVals[i * Number + j]}";
                }
            }
        }

        /// <summary>
        /// �����̏d���`�F�b�N
        /// </summary>
        /// <param name="n">�`�F�b�N�Ώۂ̐���</param>
        /// <param name="x">�`�F�b�N�Ώۂ̃}�X�ڂ̓Y��</param>
        /// <returns>�^�U�l</returns>
        private bool Check(int n, int x)
        {
            var rowTop = x / Number * Number;
            var columnTop = x % Number;
            var frameTop = x - x / Number * Number % 27 - x % 3;

            for (var i = 0; i < Number; i++)
            {
                if (sqVals[rowTop + i] == n) return false;
                if (sqVals[columnTop + i * Number] == n) return false;
            }

            for (var i = 0; i < 3; i++)
            {
                for (var j = 0; j < 3; j++)
                {
                    if (sqVals[frameTop + Number * i + j] == n) return false;
                }
            }

            return true;
        }
    }
}