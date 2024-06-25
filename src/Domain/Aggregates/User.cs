using Domain.Abstractions;
using Domain.Entities;
using Domain.ValueObjects;
using Ruby.Generated;

namespace Domain.Aggregates;

[StrongId(typeof(Ulid))]
public sealed class User(UserId id) : AggregateRoot<UserId>(id)
{
    public string Username { get; init; } = default!;

    public string Email { get; init; } = default!;

    public string PasswordHash { get; private set; } = default!;

    public Level Level { get; init; } = default!;

    public Wallet Wallet { get; init; } = default!;

    public ICollection<Case> CaseInventory { get; init; } = [];

    public ICollection<Item> ItemInventory { get; init; } = [];

    public bool TryUpdatePassword(string oldPassword, string newPassword)
    {
        if (!BC.EnhancedVerify(oldPassword, PasswordHash))
            return false;

        PasswordHash = BC.EnhancedHashPassword(newPassword);
        return true;
    }

    public static User NewUser(string username, string email, string password, DateTime createdOn, string createdBy)
    {
        return new User(UserId.NewUserId())
        {
            Username = username,
            Email = email,
            PasswordHash = BC.EnhancedHashPassword(password),
            Level = new Level(0),
            Wallet = new Wallet(0),
            CaseInventory = [],
            ItemInventory = [],
            Created = createdOn,
            CreatedBy = createdBy,
        };
    }
}