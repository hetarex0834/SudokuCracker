namespace SudokuCracker
{
    /// <summary>
    /// ナンプレ解読システム
    /// </summary>
    public partial class MainForm : Form
    {
        // 盤面の長さ
        private static readonly int Len = 9;
        // 盤面
        private readonly TextBox[,] txtGrids = new TextBox[Len, Len];
        // 盤面の情報
        private readonly int[] board = new int[Len * Len];

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
            bool flg = false;
            Initialize();
            Solve(0, ref flg);
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            for (var i = 0; i < Len; i++)
            {
                for (var j = 0; j < Len; j++)
                {
                    txtGrids[i, j].Text = "";
                }
            }
        }

        private void Initialize()
        {
            for (var i = 0; i < Len; i++)
            {
                for (var j = 0; j < Len; j++)
                {
                    if (txtGrids[i, j].Text != "") board[i * Len + j] = int.Parse(txtGrids[i, j].Text);
                    else board[ i * Len + j] = 0;
                }
            }
        }

        private void Solve(int x, ref bool flg)
        {
            if (x > Len * Len - 1)
            {
                PrintBoard();
                flg = true;
            }
            else
            {
                if (board[x] != 0) Solve(x + 1, ref flg);
                else
                {
                    for (var i = 1; i <= 9; i++)
                    {
                        if (flg) break;
                        if (Check(i, x))
                        {
                            board[x] = i;
                            Solve(x + 1, ref flg);
                            board[x] = 0;
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
                    txtGrids[i, j].Text = $"{board[i * Len + j]}";
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
                if (board[rowTop + i] == n) return false;
                if (board[columnTop + i * Len] == n) return false;
            }

            for (var i = 0; i < 3; i++)
            {
                for (var j = 0; j < 3; j++)
                {
                    if (board[frameTop + Len * i + j] == n) return false;
                }
            }

            return true;
        }
    }
}