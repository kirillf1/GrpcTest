using Avalonia.Controls;
using Core.Services;
using Grpc.Net.Client;
using GRPCClient;
using UI.Services;
using Core.Contracts;
namespace UI
{
    public partial class MainWindow : Window
    {
        IAuthService authService;
        IPersonService personService;
        IRoleService roleService;
        IPersonContext personContext;
        PersonUserControll personUserControl;
        RoleDto? SelectedRole;
        public MainWindow()
        {
            InitializeComponent();
            var channel = GrpcChannel.ForAddress("https://localhost:7047");
            channel.ConnectAsync();
            var tokenProvider = new TokenProviderInMemory();
            roleService = new RoleService(new GrpcService.RoleService.RoleServiceClient(channel));
            personContext = new PersonJwtContext(roleService, tokenProvider);
            personService = new SecurityPersonServiceDecorator(
                new PersonServiceGrpc(new GrpcService.Protos.PersonService.PersonServiceClient(channel), tokenProvider),
                personContext);
            authService = new AuthServiceGrpc(
                     new GrpcService.Protos.Auth.AuthClient(channel), tokenProvider);
            var roleSelect = this.Get<ComboBox>("RoleSelect");
            roleSelect.PointerEnter+=RoleSelect_PointerEnter;
            roleSelect.SelectionChanged+=RoleSelect_SelectionChanged;
            var button = this.Get<Button>("RegisterButton");
            button.Click += Register;
            var loginButton = this.Get<Button>("LoginButton");
            loginButton.Click += Login;
            var personButton = this.Get<Button>("PersonsButton");
            personButton.Click += PersonClick;
        }

        private void RoleSelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var role = (RoleDto)e.AddedItems[0];
            SelectedRole = role;
        }

        private async void RoleSelect_PointerEnter(object sender, Avalonia.Input.PointerEventArgs e)
        {
            var roleSelect = this.Get<ComboBox>("RoleSelect");
            if (!roleSelect.Items.GetEnumerator().MoveNext())
                roleSelect.Items = await roleService.GetAllRoles();
        }

        private void PersonClick(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            personUserControl = new PersonUserControll();
            personUserControl.DataContext = new PersonsViewModel(personService, personContext);
            personUserControl.Show();

        }



        private async void Register(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var name = this.Get<TextBox>("RegisterName");
            var password = this.Get<TextBox>("RegisterPassword");
            await authService.Register(new Core.Contracts.RegisterModel()
            {
                Name = name.Text,
                Password = password.Text,
                Role =SelectedRole?.Name ?? ""
            });
            try
            {
                var personName = this.Get<TextBlock>("PersonName");
                var person = await personContext.GetPersonInfo();
                personName.Text = person.Name;
                var personButton = this.Get<Button>("PersonsButton");
                personButton.IsVisible = true;
                var roleName = this.Get<TextBlock>("RoleName");
                roleName.Text = person.RoleName;
            }
            catch { }
        }
        private async void Login(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var name = this.Get<TextBox>("LoginName");
            var password = this.Get<TextBox>("LoginPassword");
            await authService.Login(new Core.Contracts.LoginModel() { Name = name.Text, Password = password.Text });
            try
            {
                var personName = this.Get<TextBlock>("PersonName");
                var person = await personContext.GetPersonInfo();
                personName.Text = person.Name;
                var personButton = this.Get<Button>("PersonsButton");
                personButton.IsVisible = true;
                var roleName = this.Get<TextBlock>("RoleName");
                roleName.Text = person.RoleName;
            }
            catch { }
        }


    }
}
