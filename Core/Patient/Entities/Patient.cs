using Domain.Base;

namespace Domain.Patient.Entities;

public class Patient : BaseAggregateRoot<Guid>
{

    #region Fields

    public string FullName { get; private set; }

    public string NationalNo { get; private set; }

    #endregion Fields


    protected override void SetStateByEvent(IEvent @event)
    {
        throw new NotImplementedException();
    }

    protected override void ValidateInvariants()
    {
        throw new NotImplementedException();
    }
}
