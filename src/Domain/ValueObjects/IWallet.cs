using Domain.Abstractions;

namespace Domain.ValueObjects;

public interface IWallet : IValueObject
{
    /// <summary>
    /// Checks whether the wallet has enough balance to perform the transaction.
    /// </summary>
    public bool HasBalance(decimal amount);

    /// <summary>
    /// Adds the specified amount to the wallet.
    /// </summary>
    public void AddFunds(decimal amount);

    /// <summary>
    /// Removes the specified amount from the wallet.
    /// </summary>
    /// <returns>true, if the operation was successful, false otherwise.</returns>
    public bool TryRemoveFunds(decimal amount);

    /// <summary>
    /// Tries to transfer the specified amount to the target wallet.
    /// </summary>
    /// <note>
    /// this operation is atomic, it either goes through fully or fails.
    /// </note>
    /// <returns>true, if the operation was successful, false otherwise.</returns>
    public bool TryTransfer(IWallet target, decimal amount);
}