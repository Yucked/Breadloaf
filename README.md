<p align="center">
	<img src="https://i.imgur.com/4XONqbe.png" width="20%"/>
	</br>
	<a href="https://discord.gg/ZJaVXK8">
		<img src="https://img.shields.io/badge/Discord-Support-%237289DA.svg?logo=discord&style=for-the-badge&logoWidth=20&labelColor=0d0d0d" />
	</a>
  	<a href="http://buymeacoff.ee/Yucked">
		<img src="https://img.shields.io/badge/Buy%20Me%20A-Coffee-%23FF813F.svg?logo=buy-me-a-coffee&style=for-the-badge&logoWidth=20&labelColor=0d0d0d" />
	</a>
  	<a href="https://dev.azure.com/Yucked/Builds/_build?definitionId=5">
		<img src="https://img.shields.io/azure-devops/build/yucked/24313938-90f7-4803-a2c4-3f5493129c43/5?color=%23CB2E6D&label=Build%20Status&logo=azure-pipelines&logoColor=%232560E0&style=for-the-badge&labelColor=131313&logoWidth=20" />
	</a>
	<p align="center">
	     🍞 - Breadloaf is a .NET implementation of a Blockchain on Blazor server.
  </p>
</p>

---

## `📚 BACKGROUND:`
This project is part of my undergrad research t I did at my college on **Discovering Blockchain Technology**. To demonstrate how Blockchain works, I decided to build an application with these capabilities in mind:

- Is console based
- Has some sort of UI (Web Pages)
- Fast & easy to understand
- Cross platform

Ideally, ASP.NET Core would have done it but Blazor is something new that I've been wanting to try out for some time but the websocket aspect of it poses a problem to have a fully functional Blockchain application.

## `⚗️ SETUP:`
- To run this project make sure you have .NET Core Hosting Bundle installed on your machine. 
.NET Core 3.1 can be downloaded from here: [DOWNLOAD](https://dotnet.microsoft.com/download/dotnet-core/3.1)

- Once installed, open a command prompt or powershell in the project directory and run `dotnet run` to run the project.

- Open your browser and navigate to `localhost:5000` to preview the web page.

## `📚 To-Do List:`
- [ ] Proof of work implementation.
- [x] Verifying block's hashes when verifying if chain is valid.
- [x] Building a blockchain from web UI.
- [x] CSS to demonstrate an actual block.
- [ ] WebSockets client/server and syncing chains.
- [x] Broadcasting to all clients when a block is added to the chain.
- [ ] ~~Replace `System.Text.Json` with `Utf8Json`.~~
- [x] A separate webpage to visualize blockchain.
- [x] Rename blockchain to `Breadcrumbs` 🍞.
- [ ] Some sort of mining algorithm?
- [ ] Figure out how pending transactions and block transactions work.
