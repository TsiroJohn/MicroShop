using Discount.Grpc.Protos;
using System.Threading.Tasks;

namespace Basket.API.GrpcServices
{
    public class DiscountGrpcService
    {
        private readonly DiscountProtoService.DiscountProtoServiceClient discountProtoServiceClient;

        public DiscountGrpcService(DiscountProtoService.DiscountProtoServiceClient discountProtoServiceClient)
        {
            this.discountProtoServiceClient = discountProtoServiceClient;
        }

        public async Task<CouponsModel> GetDiscount(string productId)
        {
            var discountRequest = new GetDiscountRequest { ProductId = productId };

            return await discountProtoServiceClient.GetDiscountAsync(discountRequest);
        }
    }
}
