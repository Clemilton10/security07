# Identity Server 4

Extraia o `001_template_loja.zip`

```sh
dotnet new mvc -f netcoreapp3.1 -n is4
dotnet sln add is4
dotnet new -i identityserver4.templates
cd is4
dotnet new is4admin --force
rm -rf Controllers
cd ..
mkdir SkyCommerce.Loja
```

> Feche o Vs e o Code

```sh
mv SkyCommerce.CrossCutting.Services SkyCommerce.Loja
mv SkyCommerce.Data SkyCommerce.Loja
mv SkyCommerce.Domain SkyCommerce.Loja
mv SkyCommerce.Site SkyCommerce.Loja
mv SkyCommerce.Store SkyCommerce.Loja
```

No arquivo `SkyCommerce.sln` edite os caminhos incluindo `SkyCommerce.Loja\` em cada projeto:

-   SkyCommerce.Loja\SkyCommerce.Site\SkyCommerce.Site.csproj
-   SkyCommerce.Loja\SkyCommerce.Domain\SkyCommerce.Domain.csproj
-   SkyCommerce.Loja\SkyCommerce.Data\SkyCommerce.Data.csproj
-   SkyCommerce.Loja\SkyCommerce.CrossCutting.Services\SkyCommerce.CrossCutting.Services.csproj

No Vs adicione uma pasta `ECommerce` e leve para dentro dela os itens:

-   FrontEnd
-   infra
-   SkyCommerce.Domain

No Vs mova o projeto `SkyCommerce.Site` de dentro da pasta `FrontEnd` para a pasta `ECommerce` e exclua a pasta `FrontEnd`.

No Vs crie a pasta `ECommerce\Domain` e leve o projeto `SkyCommerce.Domain` para dentro dela.

No Vs crie a pasta `IdentityServer4` na raiz e leve o projeto `is4` para dentro dela.

No Vs renomeie o projeto `IdentityServer4\is4` para `IdentityServer4\SkyCommerce.SSO`.

<div style="border:1px solid red;">

No Vs clique com botão direito no projeto `IdentityServer4\SkyCommerce.SSO` e depois em `Sincronizar Namespaces`.

</div><br/>

Em `ECommerce\SkyCommerce.Site\Views\Shared\Produtos\-Layout.cshtml` exclua os Modais:

```csharp
<!-- Modal Login start -->
<div class="modal signUpContent fade" id="ModalLogin" tabindex="-1" role="dialog">
	<div class="modal-dialog">
		<div class="modal-content">
			<div class="modal-header">
				<button type="button" class="close" data-dismiss="modal" aria-hidden="true"> &times;</button>
				<h3 class="modal-title-site text-center"> Logar no SkyCommerce </h3>
			</div>
			<div class="modal-body">
				<form role="form" class="logForm" asp-controller="Account" asp-action="Login">
					<div class="form-group login-username">
						<div>
							<input name="Username" id="login-user" class="form-control input" size="20" placeholder="Usuario" type="text">
						</div>
					</div>
					<div class="form-group login-password">
						<div>
							<input name="Password" id="login-password" class="form-control input" size="20"
									placeholder="Senha" type="password">
						</div>
					</div>
					<div class="form-group">
						<div>
							<div class="checkbox login-remember">
								<label>
									<input name="RememberMe" value="true" checked="checked" type="checkbox">
									Lembrar-me
								</label>
							</div>
						</div>
					</div>
					<div>
						<div>
							<input name="submit" class="btn  btn-block btn-lg btn-primary" value="LOGIN" type="submit">
						</div>
					</div>
					<!--userForm-->
				</form>
			</div>
			<div class="modal-footer">
				<p class="text-center">
					Primeira vez? <a data-toggle="modal" data-dismiss="modal"
										href="#ModalSignup"> Cadastre-se. </a> <br>
					<a asp-controller="Account" asp-action="EsqueciSenha"> Esqueceu sua senha? </a>
				</p>
			</div>
		</div>
		<!-- /.modal-content -->

	</div>
	<!-- /.modal-dialog -->

</div>
<!-- /.Modal Login -->
<!-- Modal Signup start -->
<div class="modal signUpContent fade" id="ModalSignup" tabindex="-1" role="dialog">
	<div class="modal-dialog">
		<div class="modal-content">
			<div class="modal-header">
				<button type="button" class="close" data-dismiss="modal" aria-hidden="true"> &times;</button>
				<h3 class="modal-title-site text-center"> REGISTRAR </h3>
			</div>
			<div class="modal-body">
				<form asp-action="Registrar" asp-controller="Account">
					<div class="form-group reg-username">
						<div>
							<input name="Nome" class="form-control input" size="20" placeholder="Nome"
									type="text">
						</div>
					</div>
					<div class="form-group reg-email">
						<div>
							<input name="Email" class="form-control input" size="20" placeholder="Email" type="text">
						</div>
					</div>
					<div class="form-group reg-password">
						<div>
							<input name="Password" class="form-control input" size="20" placeholder="Senha"
									type="password">
						</div>
					</div>
					<div class="form-group reg-password">
						<div>
							<input name="ConfirmPassword" class="form-control input" size="20" placeholder="Confirme a sua senha"
									type="password">
						</div>
					</div>

					<div>
						<div>
							<input name="submit" class="btn  btn-block btn-lg btn-primary" value="CADASTRAR" type="submit">
						</div>
					</div>
					<!--userForm-->
				</form>
			</div>
			<div class="modal-footer">
				<p class="text-center">
					Já é cadastrado? <a data-toggle="modal" data-dismiss="modal" href="#ModalLogin">
						Logar
					</a>
				</p>
			</div>
		</div>
		<!-- /.modal-content -->

	</div>
	<!-- /.modal-dialog -->

</div>
```

Exclua também este trecho:

```csharp
<li class="hidden-xs">
	<a href="#" data-toggle="modal" data-target="#ModalSignup">
		Criar
		Conta
	</a>
</li>
```

Altere também o botão de logar:

```csharp
<li>
	<a href="#" data-toggle="modal" data-target="#ModalLogin">
		<span class="hidden-xs">Logar</span>
		<i class="glyphicon glyphicon-log-in hide visible-xs "></i>
	</a>
</li>
```

para:

```csharp
<li>
	<a asp-controller="Account" asp-action="Login">
		<span class="hidden-xs">Logar</span>
		<i class="glyphicon glyphicon-log-in hide visible-xs "></i>
	</a>
</li>
```

Em `ECommerce\SkyCommerce.Site\Controllers\AccountController.cs` exclua:

```csharp
[HttpGet]
[AllowAnonymous]
[Route("entrar")]
public async Task<IActionResult> Login(string returnUrl = null)
{
	// Clear the existing external cookie to ensure a clean login process
	await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

	ViewData["ReturnUrl"] = returnUrl;
	return View();
}
```

exclua também:

```csharp
private IActionResult RedirectToLocal(string returnUrl)
{
	if (Url.IsLocalUrl(returnUrl))
	{
		return Redirect(returnUrl);
	}
	else
	{
		return RedirectToAction(nameof(HomeController.Index), "Home");
	}
}
```

exclua também:

```csharp
[HttpPost]
[AllowAnonymous]
[ValidateAntiForgeryToken]
[Route("cadastro")]
public async Task<IActionResult> Registrar(RegistrarUsuarioViewModel model, string returnUrl = null)
{
	ViewData["ReturnUrl"] = returnUrl;
	if (!ModelState.IsValid)
	{
		return View(model);
	}

	var user = new IdentityUser() { UserName = model.Email, Email = model.Email };
	var result = await _userManager.CreateAsync(user, model.Password);
	if (result.Succeeded)
	{
		await _signInManager.PasswordSignInAsync(user, model.Password, true, true);
		return RedirectToAction("Index", "Home");
	}

	return View(model);
}
```

exclua também:

```csharp
[Route("esqueci-senha")]
public IActionResult EsqueciSenha()
{
	return View();
}

[HttpPost]
[AllowAnonymous]
[ValidateAntiForgeryToken]
[Route("esqueci-senha")]
public async Task<IActionResult> EsqueciSenha(EsqueciSenhaViewModel model)
{
	if (ModelState.IsValid)
	{
		var user = await _userManager.FindByEmailAsync(model.Email);
		if (user == null)
		{
			// Don't reveal that the user does not exist or is not confirmed
			return RedirectToAction(nameof(EsqueciSenhaSucesso));
		}

		// For more information on how to enable account confirmation and password reset please
		// visit https://go.microsoft.com/fwlink/?LinkID=532713
		var code = await _userManager.GeneratePasswordResetTokenAsync(user);
		var callbackUrl = Url.Action(
			action: nameof(AccountController.ResetSenha),
			controller: "Account",
			values: new { user.Id, code },
			protocol: Request.Scheme);

		// For god sake! Only for demo pourposes!
		TempData["UrlReset"] = callbackUrl;

		return RedirectToAction(nameof(EsqueciSenhaSucesso));
	}

	// If we got this far, something failed, redisplay form
	return View(model);
}

[HttpGet]
[AllowAnonymous]
[Route("esqueci-minha-senha-sucesso")]
public IActionResult EsqueciSenhaSucesso(string link)
{
	return View(link);
}

[HttpGet]
[AllowAnonymous]
[Route("trocar-minha-senha")]
public IActionResult ResetSenha(string code = null)
{
	if (code == null)
	{
		throw new ApplicationException("A code must be supplied for password reset.");
	}

	var model = new ResetPasswordViewModel { Code = code };
	return View(model);
}

[HttpPost]
[AllowAnonymous]
[ValidateAntiForgeryToken]
[Route("trocar-minha-senha")]
public async Task<IActionResult> ResetSenha(ResetPasswordViewModel model)
{
	if (!ModelState.IsValid)
	{
		return View(model);
	}
	var user = await _userManager.FindByEmailAsync(model.Email);
	if (user == null)
	{
		// Don't reveal that the user does not exist
		return RedirectToAction(nameof(ResetSenhaSucesso));
	}
	var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
	if (result.Succeeded)
	{
		return RedirectToAction(nameof(ResetSenhaSucesso));
	}
	return View();
}
[HttpGet]
[AllowAnonymous]
[Route("trocar-minha-senha-sucesso")]
public IActionResult ResetSenhaSucesso()
{
	return View();
}
```

Altere a rota `sair` para:

```csharp
[Route("sair")]
public IActionResult Sair()
{
	return SignOut("Cookies", "oidc");
}
```

Altere a rota `entrar` para:

```csharp
[HttpGet]
[Authorize]
[Route("entrar")]
public IActionResult Login(string returnUrl = null)
{
	if (returnUrl.IsPresent())
		return Redirect(returnUrl);

	return RedirectToAction("Index", "Home");
}
```

> Este método `returnUrl.IsPresent()` é da loja

e adicione o pacote no cabeçalho da controller:

```csharp
using SkyCommerce.Extensions;
```

No Vs exclua as seguintes páginas da pasta `ECommerce\SkyCommerce.Site\Views\Account`:

-   EsqueciSenha.cshtml
-   EsqueciSenhaSucesso.cshtml
-   Login.cshtml
-   Registrar.cshtml
-   ResetSenha.cshtml
-   ResetSenhaSucesso.cshtml

No Vs exclua as seguintes páginas da pasta `ECommerce\SkyCommerce.Site\Models`:

-   EsqueciSenhaViewModel.cs
-   LoginViewModel.cs
-   RegistrarUsuarioViewModel.cs
-   ResetPasswordViewModel.cs

Na `ECommerce\SkyCommerce.Site\Startup.cs` exclua a configuração do identity server existente:

```csharp
// ASP.NET Identity Configuration
services
	.AddIdentity<IdentityUser, IdentityRole>(o =>
	{
		o.Password.RequireDigit = false;
		o.Password.RequireLowercase = false;
		o.Password.RequireNonAlphanumeric = false;
		o.Password.RequiredLength = 8;
		o.Password.RequireUppercase = false;
		o.SignIn.RequireConfirmedEmail = false;
		o.SignIn.RequireConfirmedAccount = false;

	})
	.AddEntityFrameworkStores<SkyContext>()
	.AddDefaultTokenProviders()
	.AddErrorDescriber<IdentityMensagensPortugues>();
```

Em `ECommerce\infra\SkyCommerce.Data\Context\SkyContext.cs` substitua `IdentityDbContext<IdentityUser>` por `DbContext`.

Em `ECommerce\infra\SkyCommerce.Data\SkyCommerce.Data.csproj` exclua a dependência:

```xml
<PackageReference Include="Microsoft.AspNetCore.Identity.EntityFrameworkCore" Version="3.1.3" />
```

Em `ECommerce\infra\SkyCommerce.Data\Util\FakeData.cs` exclua o método:

```csharp
/// <summary>
/// Generate default admin user / role
/// </summary>
public static async Task EnsureSeedIdentityData(UserManager<IdentityUser> userManager, IConfiguration configuration)
{

	// Create user
	if (await userManager.FindByNameAsync(configuration.GetValue<string>("ApplicationSettings:DefaultUser") ?? "bruno") != null) return;

	var user = new IdentityUser()
	{
		UserName = configuration.GetValue<string>("ApplicationSettings:DefaultUser") ?? "bob@bob.com",
		Email = configuration.GetValue<string>("ApplicationSettings:DefaultEmail") ?? "bob@bob.com",
		EmailConfirmed = true,
		LockoutEnd = null
	};

	await userManager.CreateAsync(user, configuration.GetValue<string>("ApplicationSettings:DefaultPass") ?? "10203040");
}
```

Em `ECommerce\SkyCommerce.Site\Configure\DatabaseChecker.cs` exclua os trechos:

```csharp
var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
```

```csharp
Log.Information("Carregando usuarios");
await FakeData.EnsureSeedIdentityData(userManager, configuration);
```

Em `ECommerce\SkyCommerce.Site\Controllers\AccountController.cs` exclua o trecho:

```csharp
private readonly UserManager<IdentityUser> _userManager;
private readonly SignInManager<IdentityUser> _signInManager;
private readonly ILogger<AccountController> _logger;

public AccountController(
	UserManager<IdentityUser> userManager,
	SignInManager<IdentityUser> signInManager,
	ILogger<AccountController> logger)
{
	_userManager = userManager;
	_signInManager = signInManager;
	_logger = logger;
}
```

Em `ECommerce\SkyCommerce.Site\SkyCommerce.Site.csproj` adicione o pacote:

```xml
<PackageReference Include="Microsoft.AspNetCore.Authentication.OpenIdConnect" Version="3.1.8" />
```

Em `ECommerce\SkyCommerce.Site\Startup.cs` adicione depois de `services.AddHttpClient();`:

```csharp
services.AddHttpContextAccessor();
JwtSecurityTokenHandler.DefaultMapInboundClaims = false;
if (Debugger.IsAttached)
	IdentityModelEventSource.ShowPII = true;
services.AddAuthentication(o =>
{
	o.DefaultScheme = "Cookies";
	o.DefaultChallengeScheme = "oidc";
})
.AddCookie("Cookies")
.AddOpenIdConnect("oidc", options =>
{
	options.Authority = "https://localhost:5001";
	options.ClientId = "e94ac8054a74483ea83f2fe0f406a7d6";
	options.ClientSecret = "4bf865da9918453aa568e7579489fb34";
	options.ResponseType = "code";
	options.Scope.Add("profile");
	options.Scope.Add("openid");
	options.SaveTokens = true;
	options.GetClaimsFromUserInfoEndpoint = true;
	options.TokenValidationParameters = new TokenValidationParameters()
	{
		NameClaimType = "name",
		RoleClaimType = "role"
	};
});
```

Suba o projeto `IdentityServer4\SkyCommerce.SSO`, vá na aba `Clients` e adicione um cliente em `Add Client`:

-   Clique em `Web App`, depois em `Start`:

|            Campo | Resposta                                                               |
| ---------------: | :--------------------------------------------------------------------- |
|       Client ID: | Clique no <b>botão aleatório</b> da direita para gerar automaticamente |
|    Display Name: | SkyCommerce                                                            |
|     Display URL: | https://skyCommerce.desenvolvedor.io                                   |
|        Logo URL: |                                                                        |
|     Description: |                                                                        |
| REquire Consent: | Sim                                                                    |

Em `ECommerce\SkyCommerce.Site\Startup.cs` altere:

```csharp
options.ClientId = "e94ac8054a74483ea83f2fe0f406a7d6";
```

-   Clique em `Next`

|         Campo | Resposta                           |
| ------------: | :--------------------------------- |
| Callback URL: | https://localhost:5006/signin-oidc |

-   Clique em `Next`

|       Campo | Resposta                                     |
| ----------: | :------------------------------------------- |
| Logout URL: | https://localhost:5006/signout-callback-oidc |

-   Clique em `Next`

|            Campo | Resposta                                                               |
| ---------------: | :--------------------------------------------------------------------- |
|            Type: | Shared Secret                                                          |
| Expiration Date: | Não                                                                    |
|           Value: | Clique no <b>botão aleatório</b> da direita para gerar automaticamente |
|     Description: |                                                                        |

Em `ECommerce\SkyCommerce.Site\Startup.cs` altere:

```csharp
options.ClientSecret = "4bf865da9918453aa568e7579489fb34";
```

-   Clique em `Add`
-   Clique em `Next`

-   Marque os escopos e os transfira para o lado direito:

    -   profile
    -   openid

-   Clique em `Next`

-   Não marque nenhuma Resource, clique em `Next`

-   Clique em `Salvar`

Suba o projeto `IdentityServer4\SkyCommerce.SSO`, vá na aba `Users` e adicione um cliente em `Add User`:

|          Campo | Resposta    |
| -------------: | :---------- |
|    First Name: | Bob         |
|     Last Name: | Smith       |
|      Username: | bob         |
| Email Address: | bob@bob.com |
|      Password: | Senha123!   |
