// using AtosLearningAPI.Model;
// using MySql.Data.MySqlClient;
//
// namespace AtosLearningAPI.Data.Repositories;
//
// public class CourseRepository : ICourseRepository
// {
//     private readonly MySQLConfiguration _mySQLConfiguration;
//     
//     public CourseRepository(MySQLConfiguration mySQLConfiguration)
//     {
//         _mySQLConfiguration = mySQLConfiguration;
//     }
//     
//     protected MySqlConnection GetConnection() => new MySqlConnection(_mySQLConfiguration.ConnectionString);
//     
//     public async Task<IEnumerable<Course>> GetCourses()
//     {
//         var db = GetConnection();
//         var command = "SELECT course_id Id, course_name Name, course_code Code FROM Courses";
//     }
//     
//     public Task<Course> GetCourse(string id);
//     public Task<Course> CreateCourse(Course course);
//     public Task UpdateCourse(Course course);
//     public Task DeleteCourse(string id);
// }