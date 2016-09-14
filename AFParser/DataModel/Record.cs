using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFParser.DataModel
{
    public class Record
    {

        public Record()
        {
            //..
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            if (Title != null)
            {
                sb.Append("Title: " + Title + Environment.NewLine);
            }
            if (NomeDeLaRevue != null)
            {
                sb.Append("NomeDeLaRevue: " + NomeDeLaRevue + Environment.NewLine);
            }
            if (Editeur != null)
            {
                sb.Append("Editeur: " + Editeur + Environment.NewLine);
            }
            if (PublicationDate != null)
            {
                sb.Append("PublicationDate: " + PublicationDate + Environment.NewLine);
            }
            if (PagesNumber != null)
            {
                sb.Append("PagesNumber: " + PagesNumber + Environment.NewLine);
            }
            if (CallN != null)
            {
                sb.Append("CallN: " + CallN + Environment.NewLine);
            }
            if (Commentary != null)
            {
                sb.Append("Commentary: " + Commentary + Environment.NewLine);
            }
            if (Authors != null)
            {
                sb.Append("Authors: " + Authors + Environment.NewLine);
            }
            if (ISBN != null)
            {
                sb.Append("ISBN: " + ISBN + Environment.NewLine);
            }

            return sb.ToString();
        }

        public string ISBN
        {
            set;
            get;
        }

        public string Authors
        {
            set;
            get;
        }

        public string Commentary
        {
            set;
            get;
        }

        public string CallN
        {
            set;
            get;
        }

        public string PagesNumber
        {
            set;
            get;
        }

        public string PublicationDate
        {
            set;
            get;
        }

        public string Editeur
        {
            set;
            get;
        }

        public string NomeDeLaRevue
        {
            set;
            get;
        }

        public string Title
        {
            set;
            get;
        }
    }
}
