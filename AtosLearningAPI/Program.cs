using AtosLearningAPI.Data;
using AtosLearningAPI.Data.Repositories;
using AtosLearningAPI.Data.Repositories.QuestionStat;
using AtosLearningAPI.Model;
using MySql.Data.MySqlClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var mySqlConfig = new MySQLConfiguration(builder.Configuration.GetConnectionString("DefaultConnection"));
builder.Services.AddSingleton(mySqlConfig);

//builder.Services.AddSingleton(new MySqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ISubjectRepository, SubjectRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IVideoGameExamRepository, VideoGameExamRepository>();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<IExamRepository, ExamRepository>();
builder.Services.AddScoped<IExamSubmissionRepository, ExamSubmissionRepository>();
builder.Services.AddScoped<IQuestionStatRepository, QuestionStatRepository>();

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();