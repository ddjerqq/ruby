using Domain.Abstractions;

namespace Domain.ValueObjects;

public sealed record Wallet(decimal Balance) : IWallet
{
    public decimal Balance { get; private set; } = Balance;

    public bool HasBalance(decimal amount) => Balance >= amount;
    public void AddFunds(decimal amount) => Balance += amount;


    public bool TryRemoveFunds(decimal amount)
    {
        if (!HasBalance(amount))
            return false;

        Balance -= amount;
        return true;
    }

    public bool TryTransfer(IWallet other, decimal amount)
    {
        if (!this.TryRemoveFunds(amount))
            return false;

        other.AddFunds(amount);

        return true;
    }

    public static implicit operator decimal(Wallet wallet) => wallet.Balance;

    public static explicit operator Wallet(decimal balance) => new(balance);
}