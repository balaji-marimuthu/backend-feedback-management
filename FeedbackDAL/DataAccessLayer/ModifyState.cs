using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeedbackDAL.DataAccessLayer
{
    [ExcludeFromCodeCoverage]
    public class ModifyState
    {
        public virtual void SetEntityStateModified<T>(FeedbackContext db, T item) where T : class
        {
            db.Entry(item).State = System.Data.Entity.EntityState.Modified;
        }
    }
}
