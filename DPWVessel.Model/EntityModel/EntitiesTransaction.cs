﻿using System;
using System.Data.Entity;

namespace DPWVessel.Model.EntityModel
{
    public partial class dpw_VesselEntities
    {
        private DbContextTransaction _transaction;
        public void StartTransaction()
        {
            if (_transaction != null)
                throw new ApplicationException("A transaction is already started");
            _transaction = this.Database.BeginTransaction();
        }
        public void CommitTransaction()
        {
            if (_transaction == null)
                throw new ApplicationException("No transaction");
            _transaction.Commit();
            _transaction = null; //transcation not disposed, could be memory leak???
            //_transaction.Dispose();
        }
        public void RollBackTransaction()
        {
            if (_transaction == null)
                throw new ApplicationException("No transaction");
            _transaction.Rollback();
            _transaction = null;
            //_transaction.Dispose();
        }
        public bool IsTransactionStarted { get { return this._transaction != null; } }

        protected override void Dispose(bool disposing)
        {
            if (_transaction != null)
                _transaction.Dispose();
            base.Dispose(disposing);
        }
    }

}
