# ImageWatermarkApi (API REST para Manipulação de Imagens)

## Índice

- [Sobre](#sobre)
- [Funcionalidades](#funcionalidades)
  [Tecnologias Utilizadas](#tecnologias-utilizadas)
- [Pré-Requisitos](#pré-requisitos)
- [Configuração do Ambiente de Desenvolvimento](#configuração-do-ambiente-de-desenvolvimento)
- [Como Iniciar o Projeto](#como-iniciar-o-projeto)
- [Contribuição](#contribuição)

## Sobre

Este projeto é uma API REST simples desenvolvida utilizando .NET 8 que permite a manipulação de imagens. A API recebe uma imagem codificada em base64, decodifica essa imagem, aplica uma marca d'água configurável e retorna a imagem resultante em base64.

## Funcionalidades

- Receber Imagem: Endpoint HTTP POST que aceita uma string em base64 representando uma imagem.
- Decodificação: Converte a string base64 recebida de volta para o formato de imagem original (PNG, JPEG, etc.).
- Marca d'Água: Aplica uma marca d'água configurável na imagem decodificada.
- Retorno da Imagem: Converte a imagem com a marca d'água para base64 e retorna ao solicitante.

## Tecnologias Utilizadas

- .NET Core
- SixLabors.ImageSharp
- System.Drawing.Common
- Microsoft.VisualStudio.Azure.Containers.Tools.Targets
- SixLabors.Fonts
- Swashbuckle.AspNetCore
- Mvc

## Pré-Requisitos

- .NET 8 SDK
- Visual Studio 2022 ou superior
- Docker Desktop

## Configuração do Ambiente de Desenvolvimento
- Clone o projeto na máquina local 🗂️
- Abra a solução "ImageWatermarkApi.sln" no Visual Studio 📂
- Atualizar os pacotes no Nuget, se necessários 🗃️

## Como Iniciar o Projeto 

- Executar (escolha http/https, iee ou docker)🚀
- Acesse o endpoint HTTP POST para enviar uma imagem codificada em base64 e receber a imagem com a marca d'água aplicada
- Corpo da requisição (JSON):
```
  {
    "Base64Image": "sua_imagem_base64",
    "WatermarkText": "Nome da imagem",
    "Position": "Posição da assinatura"
  }  
```
- Posições disponíveis: topleft, topright, bootomleft ou bootomright
  
## Contribuição

Contribuições são bem-vindas! Sinta-se à vontade para abrir problemas, enviar solicitações de pull e participar do desenvolvimento deste projeto. Consulte o [CONTRIBUTING.md](CONTRIBUTING.md#como-contribuir)
 para obter orientações sobre como contribuir.
