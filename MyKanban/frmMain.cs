using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyKanban
{
    public partial class frmMain : Form
    {
        private model.Board modelBoard = null;
        private List<model.BoardModel> listBoardModel = null;

        public frmMain()
        {
            InitializeComponent();
        }

        private void UnitTest()
        {
            //lib.mySQLiteConnect.Connect();
            //Console.WriteLine(lib.mySQLiteConnect.isConnected.ToString());            
            //int i = 0;
            //model.BoardModel board;
            //while (i < 100)
            //{
            //    board = brd.LoadBoardID(1);
            //    Console.WriteLine("ID : " + board.ID.ToString() + ", NAME: " + board.Name + ", DESC: " + board.Description);
            //    i = i + 1;
            //}
            //try
            //{
            //    board = brd.LoadBoardID(2);
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
        }

        private void Initialize()
        {
            // connect to the database first
            lib.mySQLite.Connect();
            if (!lib.mySQLite.isConnected)
            {
                throw new Exception("Unable to connect to the database.");
            }

            // once connect to database, we can initialize the rest of the object that we
            // needed on the main form.
            this.modelBoard = new model.Board();
            this.listBoardModel = this.modelBoard.ListBoard();
        }

        private void Populate()
        {
            // once we finished initialized all the object and component
            // then we can fill the components with all the data we already got during
            // initialize process.

            for (int i = 0; i < this.listBoardModel.Count; i++)
            {
                this.cmbName.Items.Add(this.listBoardModel[i].ID.ToString() + " | " + this.listBoardModel[i].Name);
            }
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            // remark this once we finished with all the testing
            UnitTest();

            // initialize the program
            this.Initialize();
            // populate the components
            this.Populate();
        }

        private void cmbName_SelectedIndexChanged(object sender, EventArgs e)
        {
            string currSelection = this.cmbName.Text;

            // get the selected BoardID
            string[] strBoardID = currSelection.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
            int BoardID = -1;
            int.TryParse(strBoardID[0], out BoardID);

            // get the BoardID from database
            try
            {
                model.BoardModel res = this.modelBoard.LoadBoardID(BoardID);
                this.lblDescription.Text = res.Description;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Loading Board", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
#if DEBUG
            Console.WriteLine("frmMain: Load Board ID = " + strBoardID[0]);
#endif
        }
    }
}
