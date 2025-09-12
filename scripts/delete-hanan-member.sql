-- Script para deletar o membro Hanan existente e suas dependências
-- Execute este script antes de testar a criação de um novo membro Hanan

-- 1. Deletar assignments (papéis) do membro Hanan
DELETE FROM "Assignments"
WHERE "MemberId" IN (
    SELECT m."Id"
    FROM "Members" m
    WHERE m."FirstName" = 'Hanan'
    AND m."LastName" = 'Del Chiaro'
);

-- 2. Deletar credenciais de usuário do membro Hanan
DELETE FROM "UserCredentials"
WHERE "MemberId" IN (
    SELECT m."Id"
    FROM "Members" m
    WHERE m."FirstName" = 'Hanan'
    AND m."LastName" = 'Del Chiaro'
);

-- 3. Deletar contatos do membro Hanan
DELETE FROM "Contacts"
WHERE "EntityId" IN (
    SELECT m."Id"
    FROM "Members" m
    WHERE m."FirstName" = 'Hanan'
    AND m."LastName" = 'Del Chiaro'
)
AND "EntityType" = 'Member';

-- 4. Deletar endereços do membro Hanan
DELETE FROM "Addresses"
WHERE "EntityId" IN (
    SELECT m."Id"
    FROM "Members" m
    WHERE m."FirstName" = 'Hanan'
    AND m."LastName" = 'Del Chiaro'
)
AND "EntityType" = 'Member';

-- 5. Deletar o membro Hanan
DELETE FROM "Members"
WHERE "FirstName" = 'Hanan'
AND "LastName" = 'Del Chiaro';

-- Verificar se a deleção foi bem-sucedida
SELECT 'Membro Hanan deletado com sucesso' as Status;
