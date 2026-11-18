namespace CourseApp;

public interface IIdentifiable<TKey>
{
    TKey Id { get; }
}
