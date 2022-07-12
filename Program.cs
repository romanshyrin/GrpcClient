using Grpc.Net.Client;

namespace GrpcClient;

 class Program
    {
        static async Task Main(string[] args)
        { 

            var channel = GrpcChannel.ForAddress("https://localhost:7219");
            
            string exit;
            do
            {
                Console.WriteLine("Список студентов: ");
                await displayAllStudents(channel);
                Console.WriteLine("Выберите следующее действие:");
                Console.WriteLine("Добавить нового студента - enter  (NEW)");
                Console.WriteLine("Удалить данные о студенте - enter (DEL)");
                Console.WriteLine("Редактировать - enter  (EDIT)");
                Console.WriteLine("Выход? Y/N");
          
            string ans = Console.ReadLine();
            switch (ans)
            {
                case "NEW":
                    Console.WriteLine("Добавление нового студента:");
                    Console.WriteLine("Имя:");
                    string n_fn = Console.ReadLine();
                    Console.WriteLine("Фамилия:");
                    string n_ln = Console.ReadLine();
                    Console.WriteLine("Email:");
                    string n_em = Console.ReadLine();
                    
                    StudentModel newStudent = new StudentModel()
                    {
                        FirstName = n_fn,
                        LastName = n_ln,
                        Email = n_em,
                    };
                    await insertStudent(channel, newStudent);
                    break;
                
                case "DEL":
                    Console.WriteLine("Удалить студента с ID?:");
                    int d_id = Convert.ToInt32(Console.ReadLine());
                    await deleteStudent(channel, d_id);
                    break;
                case "EDIT":
                    Console.WriteLine("Изменение данных студента:");
                    Console.WriteLine("Номер студента?:");
                    int e_id = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("Имя студента:");
                    string e_fn = Console.ReadLine();
                    Console.WriteLine("Фамилия фамилия:");
                    string e_ln = Console.ReadLine();
                    Console.WriteLine("Email:");
                    string e_em = Console.ReadLine();
                    
                    StudentModel updStudent = new StudentModel()
                    {
                        StudentId = e_id,
                        FirstName = e_fn,
                        LastName = e_ln,
                        Email = e_em,
                    };
                    await updateStudent(channel, updStudent);
                    break;
            }
            
            Console.WriteLine("Выход? Y/N");
            exit = Console.ReadLine();
            }
            while (exit == "N");
            Console.ReadLine();
        }

        static async Task findStudentById(GrpcChannel channel, int id)
        {
            var client = new RemoteStudent.RemoteStudentClient(channel);

            var input = new StudentLookupModel() { StudentId = id };
            var reply = await client.GetStudentInfoAsync(input);
            Console.WriteLine($"{reply.FirstName} {reply.LastName}");
        }

        static async Task insertStudent(GrpcChannel channel, StudentModel student)
        {
            var client = new RemoteStudent.RemoteStudentClient(channel);

            var reply = await client.InsertStudentAsync(student);
            Console.WriteLine(reply.Result);
        }

        static async Task updateStudent(GrpcChannel channel, StudentModel student)
        {
            var client = new RemoteStudent.RemoteStudentClient(channel);

            var reply = await client.UpdateStudentAsync(student);
            Console.WriteLine(reply.Result);
        }

        static async Task deleteStudent(GrpcChannel channel, int id)
        {
            var client = new RemoteStudent.RemoteStudentClient(channel);

            var input = new StudentLookupModel() { StudentId = id };
            var reply = await client.DeleteStudentAsync(input);
            Console.WriteLine(reply.Result);
        }

        static async Task displayAllStudents(GrpcChannel channel)
        {
            var client = new RemoteStudent.RemoteStudentClient(channel);

            var empty = new Empty();
            var list = await client.RetrieveAllStudentsAsync(empty);

            Console.WriteLine("--------------------------------------------");

            foreach (var item in list.Items)
            {
                Console.WriteLine($"{item.StudentId}: {item.FirstName} {item.LastName}");
            }
            Console.WriteLine("--------------------------------------------");
       }


    }

