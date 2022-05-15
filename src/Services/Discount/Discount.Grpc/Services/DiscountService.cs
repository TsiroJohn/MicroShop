using AutoMapper;
using Discount.Grpc.Entities;
using Discount.Grpc.Protos;
using Discount.Grpc.Repositories;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Discount.Grpc.Services
{
    public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
    {
        private readonly IDiscountRepository discountRepository;
        private readonly ILogger<DiscountService> logger;
        private readonly IMapper mapper;
        public DiscountService(IDiscountRepository discountRepository, ILogger<DiscountService> logger, IMapper mapper)
        {
            this.discountRepository = discountRepository;
            this.logger = logger;
            this.mapper = mapper;
        }

        public override async Task<CouponsModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var Coupons = await discountRepository.GetDiscount(request.ProductId);
            if (Coupons == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Discount with ProductId={request.ProductId} is not found."));
            }
            logger.LogInformation("Discount is retrieved for ProductId : {ProductId}, Amount : {amount}", Coupons.ProductId, Coupons.Amount);

            var CouponsModel = mapper.Map<CouponsModel>(Coupons);
            return CouponsModel;
        }

        public override async Task<CouponsModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var coupon = mapper.Map<Coupons>(request.Coupon);

            await discountRepository.CreateDiscount(coupon);
            logger.LogInformation("Discount is successfully created. ProductId : {ProductId}", coupon.ProductId);

            var CouponsModel = mapper.Map<CouponsModel>(coupon);
            return CouponsModel;
        }

        public override async Task<CouponsModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var coupon = mapper.Map<Coupons>(request.Coupon);

            await discountRepository.UpdateDiscount(coupon);
            logger.LogInformation("Discount is successfully updated. ProductId : {ProductId}", coupon.ProductId);

            var CouponsModel = mapper.Map<CouponsModel>(coupon);
            return CouponsModel;
        }

        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var deleted = await discountRepository.DeleteDiscount(request.ProductId);
            var response = new DeleteDiscountResponse
            {
                Success = deleted
            };

            return response;
        }
    }
}
