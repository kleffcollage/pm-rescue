namespace PropertyMataaz.Models.UtilityModels
{
    public class RepositoryResponse<T>
    {
        public bool Succeeded {get;set;}
        public string ErrorMessage {get;set;} = null;
        public T Data {get;set;} 

        public static  RepositoryResponse<T> Create(T Data,bool Succeeded = true, string ErrorMessage = null)
            => Create(Data,Succeeded,ErrorMessage);
        public static TResponse Create<TResponse>(T Data,bool Succeeded, string ErrorMessage)
        where TResponse : RepositoryResponse<T>, new()
        => new TResponse{
            Succeeded = Succeeded,
            ErrorMessage = ErrorMessage,
            Data = Data
        };
    }
    
}