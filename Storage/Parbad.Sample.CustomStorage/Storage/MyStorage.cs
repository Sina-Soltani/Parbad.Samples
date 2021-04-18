using Parbad.Storage.Abstractions;
using Parbad.Storage.Abstractions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Parbad.Sample.CustomStorage.Storage
{
    public class MyStorage : IStorage
    {
        // In-Memory data
        private static readonly IList<Payment> StaticPayments = new List<Payment>();
        private static readonly IList<Transaction> StaticTransactions = new List<Transaction>();

        public Task CreatePaymentAsync(Payment payment, CancellationToken cancellationToken = new CancellationToken())
        {
            payment.Id = GenerateNewPaymentId();

            StaticPayments.Add(payment);

            return Task.CompletedTask;
        }

        public Task UpdatePaymentAsync(Payment payment, CancellationToken cancellationToken = new CancellationToken())
        {
            var record = StaticPayments.SingleOrDefault(model => model.Id == payment.Id);

            if (record == null) throw new Exception();

            record.Token = payment.Token;
            record.TrackingNumber = payment.TrackingNumber;
            record.TransactionCode = payment.TransactionCode;

            return Task.CompletedTask;
        }

        public Task DeletePaymentAsync(Payment payment, CancellationToken cancellationToken = new CancellationToken())
        {
            var record = StaticPayments.SingleOrDefault(model => model.Id == payment.Id);

            if (record == null) throw new Exception();

            StaticPayments.Remove(record);

            return Task.CompletedTask;
        }

        public Task CreateTransactionAsync(Transaction transaction, CancellationToken cancellationToken = new CancellationToken())
        {
            transaction.Id = GenerateNewTransactionId();

            StaticTransactions.Add(transaction);

            return Task.CompletedTask;
        }

        public Task UpdateTransactionAsync(Transaction transaction, CancellationToken cancellationToken = new CancellationToken())
        {
            var record = StaticTransactions.SingleOrDefault(model => model.Id == transaction.Id);

            if (record == null) throw new Exception();

            record.IsSucceed = transaction.IsSucceed;

            return Task.CompletedTask;
        }

        public Task DeleteTransactionAsync(Transaction transaction, CancellationToken cancellationToken = new CancellationToken())
        {
            var record = StaticTransactions.SingleOrDefault(model => model.Id == transaction.Id);

            if (record == null) throw new Exception();

            StaticTransactions.Remove(record);

            return Task.CompletedTask;
        }

        public IQueryable<Payment> Payments => StaticPayments.AsQueryable();

        public IQueryable<Transaction> Transactions => StaticTransactions.AsQueryable();

        private static long GenerateNewPaymentId()
        {
            return StaticPayments.Count == 0
                ? 1
                : StaticPayments.Max(model => model.Id) + 1;
        }

        private static long GenerateNewTransactionId()
        {
            return StaticTransactions.Count == 0
                ? 1
                : StaticTransactions.Max(model => model.Id) + 1;
        }
    }
}
