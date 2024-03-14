using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Domain.Exceptions;

namespace Domain.SeedWork;

/// <summary>
/// Provides a base class for defining enumeration types within the Domain-Driven Design (DDD) pattern.
/// </summary>
/// <remarks>
/// This abstract class serves as a foundation for creating custom enumeration types within your domain model. 
/// It enforces specific behavior for enumerations, including:
///   - Immutable properties for ID and Name.
///   - Overridden ToString() method to return the enumeration name.
///   - Static methods for retrieving all enumeration values, finding by name (case-insensitive), and finding by ID.
/// </remarks>
public abstract class Enumeration(int id, string name)
{
    /// <summary>
    /// The unique identifier of the enumeration value.
    /// </summary>
    public int Id { get; } = id;

    /// <summary>
    /// The human-readable name of the enumeration value.
    /// </summary>
    public string Name { get; } = name;

    /// <summary>
    /// Returns the enumeration name as a string.
    /// </summary>
    /// <returns>The name of the enumeration value.</returns>
    public override string ToString()
    {
        return Name;
    }
    
    /// <summary>
    /// Retrieves a collection of all enumeration values for a specific derived type.
    /// </summary>
    /// <typeparam name="T">The type of enumeration to retrieve values for (must inherit from Enumeration).</typeparam>
    /// <returns>An IEnumerable collection containing all enumeration values of type T.</returns>
    public static IEnumerable<T> GetAll<T>() where T : Enumeration
    {
        var fields = typeof(T).GetFields(BindingFlags.Public
                                         | BindingFlags.Static | BindingFlags.DeclaredOnly);

        return fields.Select(f => f.GetValue(null)).Cast<T>();
    }

    /// <summary>
    /// Finds the first enumeration value of a specific type that satisfies a given predicate.
    /// </summary>
    /// <typeparam name="T">The type of enumeration to search within (must inherit from Enumeration).</typeparam>
    /// <param name="predicate">A function that defines the search criteria for the enumeration value.</param>
    /// <returns>The first enumeration value of type T that matches the predicate, or null if not found.</returns>
    private static T Parse<T>(Func<T, bool> predicate) where T : Enumeration
    {
        return GetAll<T>().FirstOrDefault(predicate);
    }

    /// <summary>
    /// Finds the enumeration value of a specific type with a matching name (case-insensitive).
    /// </summary>
    /// <typeparam name="T">The type of enumeration to search within (must inherit from Enumeration).</typeparam>
    /// <param name="name">The name of the enumeration value to find.</param>
    /// <returns>The enumeration value of type T with the matching name, or throws a DomainException if not found.</returns>
    /// <exception cref="DomainException">Thrown if no enumeration value with the specified name is found.</exception>
    public static T FromName<T>(string name) where T : Enumeration
    {
        var state = Parse<T>(s => string.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

        if (state == null)
        {
            throw new DomainException($"Possible values for {typeof(T)}: {string.Join(",", GetAll<T>().Select(s => s.Name))}");
        }

        return state;
    }
    
    /// <summary>
    /// Finds the enumeration value of a specific type with a matching ID.
    /// </summary>
    /// <typeparam name="T">The type of enumeration to search within (must inherit from Enumeration).</typeparam>
    /// <param name="id">The ID of the enumeration value to find.</param>
    /// <returns>The enumeration value of type T with the matching ID, or throws a DomainException if not found.</returns>
    /// <exception cref="DomainException">Thrown if no enumeration value with the specified ID is found.</exception>
    public static T FromId<T>(int id) where T : Enumeration
    {
        var state = Parse<T>(s => s.Id == id);

        if (state == null)
        {
            throw new DomainException($"Possible values for {typeof(T)}: {string.Join(",", GetAll<T>().Select(s => s.Id))}");
        }

        return state;
    }
}
