# BURGER API

Uma api criada para serve de base para um sistema básico de fast food, com pagamento, leitura e verificação de JWT e Webhook.

### Funcionalidades:
- Pagamentos
    - Utilizando sistema do Stripe para efetuar os pagamentos.
- Webhooks
- OpenAPI/Swagger
- JWT
- Envio e confirmação de Email

### Estado Atual: <b>Pre-Alpha</b>

Arquitetura utilizada: MVC + Arquitetura Limpa

## Padrão de Pastas:
- App ->
- Models ->
    - Entities
- Utils
    - Cipher Encrypt
- Controllers
    - Dto
- Identity
- Middlewares

## Atividades

- [ ] - Rotas do Produtos
    - [ ] - Adicionar
    - [ ] - Editar
    - [ ] - Excluir
    - [ ] - Buscar Todos
- [ ] - Rotas de Usuário
    - [ ] - Login
    - [ ] - Registro
    - [ ] - Edição
    - [ ] - Confirmação de Email
- [ ] - Rotas de Pagamento
    - [ ] - Criar uma ordem
    - [ ] - Editar ordem
    - [ ] - Mover para finalizado/entregue
    - [ ] - Apagar ordem
- [ ] - Webhook para o Stripe
    - [ ] - Verificar e Atualizar a ordem
    - [ ] - Criar um id para o vínculo do usuário