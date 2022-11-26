using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Checkout.Custom.PaymentGateway.Data.Repository;
using Checkout.Custom.PaymentGateway.Dto;
using Checkout.Custom.PaymentGateway.Entity;
using Checkout.Custom.PaymentGateway.Services.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Checkout.Custom.PaymentGateway.Services.Services
{
    public interface IPaymentTransactionsService
    {
        Task<PaymentTransactionDto> AddAsync(PaymentTransactionDto transactionItem);
        Task<PaymentTransactionDto> GetByTransactionIdAsync(Guid id);
        Task<List<PaymentTransactionDto>> GetByMerchantIdAsync(Guid id);
    }

    public class PaymentTransactionsService : IPaymentTransactionsService
    {
        private readonly IPaymentTransactionRepository _paymentTransactionRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<PaymentTransactionsService> _logger;

        public PaymentTransactionsService(
            IPaymentTransactionRepository paymentTransactionRepository,
            IMapper mapper,
            ILogger<PaymentTransactionsService> logger)
        {
            _paymentTransactionRepository = paymentTransactionRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PaymentTransactionDto> AddAsync(PaymentTransactionDto transactionItem)
        {
            var transaction = _mapper.Map<PaymentTransaction>(transactionItem);
            await _paymentTransactionRepository.AddAsync(transaction);
            return transactionItem;
        }

        public async Task<PaymentTransactionDto> GetByTransactionIdAsync(Guid id)
        {
            var transaction = await _paymentTransactionRepository.GetAsync(id);
            return _mapper.Map<PaymentTransactionDto>(transaction);
        }

        public async Task<List<PaymentTransactionDto>> GetByMerchantIdAsync(Guid id)
        {
            var transactions = await _paymentTransactionRepository.GetTransactionsByMerchantId(id);
            return _mapper.Map<List<PaymentTransactionDto>>(transactions);
        }
    }


}
