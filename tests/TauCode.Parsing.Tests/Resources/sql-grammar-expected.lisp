;;; splitting comment
(:defblock create :is-top t
	(:word "CREATE")
	(:alt (:block create-table) (:block create-index))
)

;;; splitting comment
(:defblock create-table
	(:word "TABLE")
	(:alt (:some-ident table-name-ident) (:some-word table-name-word))
	(:symbol "(")
	(:block column-def :links table-closing next)
	(:symbol "," :links column-def next)
	(:block constraint-defs)
	(:symbol ")" :name table-closing)
)

;;; splitting comment
(:defblock column-def
	(:alt (:some-ident column-name-ident) (:some-word column-name-word))
	(:alt (:some-ident type-name-ident) (:some-word type-name-word))
	(:opt 
		(:symbol "(")
		(:some-int :name precision)
		(:opt
			(:symbol ",")
			(:some-int :name scale)
		)
		(:symbol ")")
	)
	(:opt
		(:alt
			(:word "NULL" :name null)
			(:seq
				(:word "NOT")
				(:word "NULL" :name not-null)
			)
		)
	)
	(:opt
		(:word "PRIMARY")
		(:word "KEY" :name inline-primary-key)
	)
	(:opt
		(:word "DEFAULT")
		(:alt
			(:word "NULL" :name default-null)
			(:some-int :name default-integer)
			(:some-string :name default-string)
		)
	)
)

;;; splitting comment
(:defblock constraint-definitions
	(:word "CONSTRAINT" :name constraint)
	(:alt (:some-ident constraint-name-ident) (:some-word constraint-name-word))
	(:alt (:block primary-key) (:some-word foreign-key))
	(:symbol "," :links constraint)
	(:idle)
)

;;; splitting comment
(:defblock primary-key
	(:word "PRIMARY" :name do-primary-key)
	(:word "KEY")
	(:block pk-columns)
)

;;; splitting comment
(:defblock pk-columns
	(:symbol "(")
	(:alt :name pk-column-name-alternatives (:some-ident pk-column-name-ident) (:some-word pk-column-name-word))
	(:opt
		(:word "ASC" :name asc)
		(:word "DESC" :name desc)
	)
	(:alt
		(:symbol "," :links pk-column-name-alternatives)
		(:idle)
	)
	(:symbol ")")
)

;;; splitting comment
(:defblock foreign-key
	(:word "FOREIGN" :name do-primary-key)
	(:word "KEY")
	(:block fk-columns)
	(:word "REFERENCES")
	(:alt (:some-ident fk-referenced-table-name-ident) (:some-word fk-referenced-table-name-word))
	(:block fk-referenced-columns)
)

;;; splitting comment
(:defblock fk-columns
	(:symbol "(")
	(:alt :name fk-column-name-alternatives (:some-ident fk-column-name-ident) (:some-word fk-column-name-word))
	(:alt
		(:symbol "," :links fk-column-name-alternatives)
		(:idle)
	)
	(:symbol ")")
)

;;; splitting comment
(:defblock fk-referenced-columns
	(:symbol "(")
	(:alt :name fk-referenced-column-name-alternatives (:some-ident fk-referenced-column-name-ident) (:some-word fk-referenced-column-name-word))
	(:alt
		(:symbol "," :links fk-referenced-column-name-alternatives)
		(:idle)
	)
	(:symbol ")")
)

;;; splitting comment
(:defblock create-index
	(:word "UNIQUE" :name do-create-unique-index)
	(:word "INDEX" :name do-create-index)
	(:alt (:some-ident index-name-ident) (:some-word index-name-word))
	(:word "ON")
	(:alt (:some-ident index-table-name-ident) (:some-word index-table-name-word))	
)
