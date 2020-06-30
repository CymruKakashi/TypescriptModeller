using System;
using System.Runtime.Serialization;

namespace Project.Models
{
    //Extending the EF Core model to implement the interface
    [DataContract]
    public partial class Person : IPerson{

    }
    //this interface is what will be converted to a TS object. 
    //This allows for a controlled subset of the EF Core model to be exposed to TS rather than the whole thing.
    public interface IPerson
    {
        [DataMember]
        string EmailAddress { get; set; }

        [DataMember]    
        Guid Id { get; set; }

        [DataMember]    
        string FirstName { get; set; }

        [DataMember]    
        string LastName { get; set; }

        [DataMember]
        int Salary { get; set; }
        
    }
}