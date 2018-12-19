using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;

namespace MyKanban.model
{
    class Board
    {
        /// <summary>
        /// Static Board Class Model Constructor
        /// Check whether we already have connection to the 
        /// </summary>
        static Board()
        {
            if (!lib.mySQLite.isConnected)
            {
                lib.mySQLite.Connect();
                if (!lib.mySQLite.isConnected)
                {
                    throw new Exception("Cannot connect to the database.");
                }
#if DEBUG
                Console.WriteLine("model.board: Connected to database.");
#endif
            }

#if DEBUG
            else
            {
                Console.WriteLine("model.board: Already connected.");
            }
#endif
        }

        /// <summary>
        /// List all the board that exists on the database
        /// </summary>
        /// <returns></returns>
        public List<BoardModel> ListBoard()
        {
#if DEBUG
            Console.WriteLine("model.board: Load the board");
#endif
            // Get all data from Table Board.
            string SQL = "SELECT board_id, board_name, board_description FROM tbl_board";

            // execute the SQL query.
            lib.mySQLite.Command(SQL);
            SQLiteDataReader reader = lib.mySQLite.CommandExecuteReader();

            // prepare the list for the return value.
            List<BoardModel> retList = new List<BoardModel>();
            int BoardID;

            // loop until last record of the board table.
            while (reader.Read())
            {
                // fill the list with board model
                BoardModel boardData = new BoardModel();
                int.TryParse(reader["board_id"].ToString(), out BoardID);
                boardData.ID = BoardID;
                boardData.Name = (string)reader["board_name"];
                boardData.Description = (string)reader["board_description"];
                retList.Add(boardData);
            }
            
            // return the list of board that already populated above
            return retList;
        }

        public DataTable ListBoardDT()
        {
            return null;
        }

        public BoardModel LoadBoardID(int BoardID)
        {
            // ensure that BoardID is a positive integer
            if (BoardID < 0)
            {
                throw new Exception("Invalid board ID (" + BoardID.ToString() + ").");
            }

            // SQL query to get specific board data from database.
            string SQL = "SELECT board_name, board_description FROM tbl_board WHERE board_id = " + BoardID.ToString();

            // exeute the SQL query
            lib.mySQLite.Command(SQL);
            SQLiteDataReader reader = lib.mySQLite.CommandExecuteReader();

            // check whether we can get the data from the database or not?
            if (!reader.Read())
            {
                throw new Exception("No records with ID = " + BoardID.ToString() + " found on database.");
            }

            BoardModel boardModel = new BoardModel();
            boardModel.ID = BoardID;
            boardModel.Name = (string)reader["board_name"] ;
            boardModel.Description = (string)reader["board_description"];

            return boardModel;
        }
    }
}
