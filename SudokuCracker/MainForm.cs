using System.Drawing;

namespace SudokuCracker
{
    /// <summary>
    /// ナンプレ解読システム
    /// </summary>
    public partial class MainForm : Form
    {
        private static readonly int Len = 9;
        // テキストボックス
        private readonly TextBox[,] txtGrids = new TextBox[Len, Len];

        private readonly int[] boards = new int[Len * Len];

        private bool flg = false;

        public MainForm()
        {
            InitializeComponent();

            for (var i = 0; i < Len; i++)
            {
                for (var j = 0; j < Len; j++)
                {
                    txtGrids[i, j] = new TextBox()
                    {
                        ImeMode = ImeMode.Disable,
                        MaxLength = 1,
                        ShortcutsEnabled = false,
                        TabIndex = i * Len + j + 1,
                        Location = new Point(160 + 60 * j, 80 + 60 * i),
                        Size = new Size(55, 55),
                        Font = new Font("Yu Gothic UI", 18F, FontStyle.Bold, GraphicsUnit.Point),
                        TextAlign = HorizontalAlignment.Center,
                    };
                    if (i < 3 || 5 < i)
                    {
                        if (3 <= j && j <= 5) txtGrids[i, j].BackColor = Color.LightGray;
                    }
                    else
                    {
                        if (j < 3 || 5 < j) txtGrids[i, j].BackColor = Color.LightGray;
                    }
                    txtGrids[i, j].KeyPress += TextBox_KeyPress;
                    this.Controls.Add(txtGrids[i, j]);
                }
            }
        }

        private void TextBox_KeyPress(object? sender, KeyPressEventArgs e)
        {
            // backspace, deleteを有効化
            if (e.KeyChar == '\b') return;
            // 押されたキーが 1～9でない場合、イベントをキャンセル
            if (e.KeyChar < '1' || '9' < e.KeyChar) e.Handled = true;
        }

        private void BtnClack_Click(object sender, EventArgs e)
        {
            Initialize();
            Solve(0);
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {

        }

        private void Initialize()
        {
            for (var i = 0; i < Len; i++)
            {
                for (var j = 0; j < Len; j++)
                {
                    if (txtGrids[i, j].Text != "") boards[i * Len + j] = int.Parse(txtGrids[i, j].Text);
                    else boards[ i * Len + j] = 0;
                }
            }
        }

        private void Solve(int x)
        {
             var MaxBoard = Len * Len;

            if (x > MaxBoard - 1)
            {
                PrintBoard();
                flg = true;
            }
            else
            {
                if (boards[x] != 0) Solve(x + 1);
                else
                {
                    for (var i = 1; i <= 9; i++)
                    {
                        if (flg) break;
                        if (Check(i, x))
                        {
                            boards[x] = i;
                            Solve(x + 1);
                            boards[x] = 0;
                        }
                    }
                }
            }
        }

        private void PrintBoard()
        {
            for (var i = 0; i < Len; i++)
            {
                for (var j = 0; j < Len; j++)
                {
                    txtGrids[i, j].Text = $"{boards[i * Len + j]}";
                }
            }
        }

        private bool Check(int n, int x)
        {
            var rowTop = x / Len * Len;
            var columnTop = x % Len;
            var frameTop = x - x / Len * Len % 27 - x % 3;

            for (var i = 0; i < Len; i++)
            {
                if (boards[rowTop + i] == n) return false;
                if (boards[columnTop + i * Len] == n) return false;
            }

            for (var i = 0; i < 3; i++)
            {
                for (var j = 0; j < 3; j++)
                {
                    if (boards[frameTop + Len * i + j] == n) return false;
                }
            }

            return true;
        }
    }
}