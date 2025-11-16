# Trabalho de Conclusão de Curso

**Tema:** Desenvolvimento de um Jogo Sério para o Ensino de Conceitos de Estrutura de Dados

* **Autor:** *Leonardo Luz Fachel*
* **Instituição:** *Instituto Federal do Rio Grande do Sul*
* **Curso:** *Análise e Desenvolvimento de Sistemas*
* **Ano:** *2025*

* [Jogue Agora](https://play.unity.com/en/games/98bb4067-113f-4850-84d7-1bd15b186e0f/alchemy-game)
* [Responda este Formulário após Jogar - WIP](#)
* [TCC](latex/main.pdf)

## Jogo

![Game Image](./latex/images/hud_bk.png)

## Motivação

> A ideia deste trabalho surgiu após observar, de forma holística, que a
> disciplina de **Estrutura de Dados** era considerada uma das mais
> desafiadoras do curso. Diversos alunos compartilhavam essa percepção.
>
> A partir disso, foi realizada uma pesquisa para verificar se essa dificuldade
> era comum em outras instituições de ensino. Os resultados confirmaram que
> **Estrutura de Dados** é amplamente reconhecida como uma disciplina complexa.
>
> Diante desse cenário, buscou-se uma alternativa para tornar o aprendizado
> mais **prático**, **interativo** e **motivador**, resultando no
> desenvolvimento de um **Jogo Sério** voltado para o ensino desses conceitos.

## O que é um Jogo Sério?

> Um **Jogo Sério** é aquele que, além de entreter, possui objetivos
> educativos, de treinamento ou de sensibilização sobre determinado tema.

No contexto deste trabalho, o jogo foi desenvolvido como uma ferramenta para
tornar o ensino de **Estrutura de Dados** mais **motivador** e **concreto**,
enfrentando as duas principais dificuldades relatadas pelos alunos dessa
disciplina.

## Detalhes do Jogo

- **Tema:** Fantasia Medieval
- **Gênero:** Plataforma 2D

### Enredo

O jogo se passa em um mundo fantasioso onde **monstros** existem e **misturas alquímicas** são utilizadas como armas.
O personagem principal teve sua pesquisa sobre a **Pedra Filosofal** roubada por um alquimista rival e agora precisa recuperá-la.
Durante a jornada, o jogador enfrentará inimigos e desafios utilizando conceitos de **Estrutura de Dados** para progredir.

## Mecânicas Centrais

| Mecânica                   | Descrição |
|-----------------------------|------------|
| **Geração de Elementos**       | Elementos são gerados ao gastar mana e posicionados automaticamente em um inventário livre, da esquerda para a direita. |
| **Misturar Elementos**         | Elementos do mesmo tipo podem ser combinados para gerar ataques. Quanto maior a quantidade de elementos iguais na combinação, maior o dano. É necessário removê-los dos inventários para realizar a mistura. |
| **Manipulação de Inventários** | Os inventários são baseados em **estruturas de dados**, permitindo operações como **inserir**, **remover**, **ordenar** e **mover o ponteiro** (no caso de listas). Cada estrutura mantém suas características próprias. |
| **Consumíveis**                | Diversos consumíveis estão disponíveis, sendo os principais: **Inserir**, **Remover** e **Ordenar**. |
| **Fraquezas**                  | Cada inimigo possui fraquezas específicas; o jogador deve combinar elementos corretos para causar dano efetivo. |
| **Pontuação**                  | A pontuação final considera combinações corretas, combinações erradas, mortes e o tempo total para completar a fase. |

## Mecânicas Genéricas

- Movimentação 2D
- Pulo Responsivo
- Corrida
- Pontos de Controle (Checkpoints)
- Tentativas Limitadas
- Pontos de Vida (HP)
- Mana
- Pulo Duplo Situacional
- Capacidade de Aparar Certos Ataques

---

> “Ensinar com jogos é transformar o desafio em motivação e o aprendizado em conquista.”
