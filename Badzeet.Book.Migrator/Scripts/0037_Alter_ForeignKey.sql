﻿alter table book.transactions add foreign key (book_id) references book.books(id)