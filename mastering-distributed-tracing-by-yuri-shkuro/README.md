## Description

This directory of the repository is intended to be the exercises of the “**Mastering Distributed Tracing - by Yuri Shkru”** book in the C#/.NET language.

You can get this wonderful book (Mastering Distributed Tracing: Analyzing performance in microservices and complex systems) from [here](https://www.packtpub.com/product/mastering-distributed-tracing/9781788628464).  
In the book itself, the exercises are written in Go, Java and Python languages. You can see its GitHub link at [here](https://github.com/PacktPublishing/Mastering-Distributed-Tracing).

---

## How to use this repo?

All dependencies are placed in the project files (`.csproj`) as usual. The only external dependency that is needed to run the applications correctly is an instance of the MySQL database along with a bunch of seed data.

```plaintext
$ docker run -d --name mysql56 -p 3306:3306 \
            -e MYSQL_ROOT_PASSWORD=mysqlpwd mysql:5.6
            
$ docker exec -i mysql56 mysql -uroot -pmysqlpwd < database.sql
```

The `database.sql` file is at the root of `Chapter04`'s directory.

However, you need a tracing backend to see traces on the services. According to the book, I used Jaeger. To run a full functional instance of Jaeger on your development environment, execute the following Docker command:

```plaintext
$ docker run -d --name jaeger \
    -p 6831:6831/udp \
    -p 16686:16686 \
    -p 14268:14268 \
    jaegertracing/all-in-one:1.6
```

That's all !

---

## Need more information?

For any topic that you think is remarkable, you can create an issue so that we can discuss it. Correcting the code makes me happy in any way; whether it's a pull request or a note on the issue!