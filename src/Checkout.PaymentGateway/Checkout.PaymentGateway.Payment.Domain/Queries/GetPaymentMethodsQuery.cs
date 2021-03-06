﻿using Checkout.PaymentGateway.Payment.Domain.AggregatesModel.PaymentMethodAggregate;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Checkout.PaymentGateway.Payment.Domain.Queries
{
    public class GetPaymentMethodsQuery : IRequest<PaymentMethod[]>
    {

    }
}
