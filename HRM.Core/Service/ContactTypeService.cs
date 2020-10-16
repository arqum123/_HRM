using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;
using HRM.Core.DataInterfaces;
using HRM.Core.IService;
using HRM.Core.DataTransfer;
using HRM.Core.DataTransfer.ContactType;
using Validation;
using System.Linq;

namespace HRM.Core.Service
{
		
	public class ContactTypeService : IContactTypeService 
	{
		private IContactTypeRepository _iContactTypeRepository;
        
		public ContactTypeService(IContactTypeRepository iContactTypeRepository)
		{
			this._iContactTypeRepository = iContactTypeRepository;
		}
        
        public Dictionary<string, string> GetContactTypeBasicSearchColumns()
        {
            
            return this._iContactTypeRepository.GetContactTypeBasicSearchColumns();
           
        }
        
        public List<SearchColumn> GetContactTypeAdvanceSearchColumns()
        {
            
            return this._iContactTypeRepository.GetContactTypeAdvanceSearchColumns();
           
        }
        

		public ContactType GetContactType(System.Int32 Id)
		{
			return _iContactTypeRepository.GetContactType(Id);
		}

		public ContactType UpdateContactType(ContactType entity)
		{
			return _iContactTypeRepository.UpdateContactType(entity);
		}

		public ContactType UpdateContactTypeByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id)
		{
			return _iContactTypeRepository.UpdateContactTypeByKeyValue(UpdateKeyValue,Id);
		}

		public bool DeleteContactType(System.Int32 Id)
		{
			return _iContactTypeRepository.DeleteContactType(Id);
		}

		public List<ContactType> GetAllContactType()
		{
			return _iContactTypeRepository.GetAllContactType();
		}

		public ContactType InsertContactType(ContactType entity)
		{
			 return _iContactTypeRepository.InsertContactType(entity);
		}


        public DataTransfer<GetOutput> Get(string _id)
        {
            DataTransfer<GetOutput> tranfer = new DataTransfer<GetOutput>();
            System.Int32 id=0;
            if (!string.IsNullOrEmpty(_id) && System.Int32.TryParse(_id,out id))
            {
				ContactType contacttype = _iContactTypeRepository.GetContactType(id);
                if(contacttype!=null)
                {
                    tranfer.IsSuccess = true;
                    GetOutput output = new GetOutput();
                    output.CopyFrom(contacttype);
                    tranfer.Data=output ;

                }
                else
                {
                    tranfer.IsSuccess = false;
                    tranfer.Errors = new string[1];
                    tranfer.Errors[0] = "Error: No record found.";
                }             
                
            }
            else
            {
                tranfer.IsSuccess = false;
                tranfer.Errors = new string[1];
                tranfer.Errors[0] = "Error: Invalid request.";
            }
            return tranfer;
        }   
        public DataTransfer<List<GetOutput>> GetAll()
        {
            DataTransfer<List<GetOutput>> tranfer = new DataTransfer<List<GetOutput>>();
            List<ContactType> contacttypelist = _iContactTypeRepository.GetAllContactType();
            if (contacttypelist != null && contacttypelist.Count>0)
            {
                tranfer.IsSuccess = true;
                List<GetOutput> outputlist = new List<GetOutput>();
                outputlist.CopyFrom(contacttypelist);
                tranfer.Data = outputlist;

            }
            else
            {
                tranfer.IsSuccess = false;
                tranfer.Errors = new string[1];
                tranfer.Errors[0] = "Error: No record found.";
            }
            return tranfer;
        }
        public DataTransfer<PostOutput> Insert(PostInput Input)
        {
           DataTransfer<PostOutput> transer = new DataTransfer<PostOutput>();
            IList<string> errors = Validator.Validate(Input);
            if(errors.Count==0)
            {
                ContactType contacttype = new ContactType();
                PostOutput output = new PostOutput();
                contacttype.CopyFrom(Input);
                contacttype = _iContactTypeRepository.InsertContactType(contacttype);
                output.CopyFrom(contacttype);
                transer.IsSuccess = true;
                transer.Data = output;
            }
            else
            {
                transer.IsSuccess = false;
                transer.Errors = errors.ToArray<string>();
            }
            return transer;
        }

        public DataTransfer<PutOutput> Update(PutInput Input)
        {
            DataTransfer<PutOutput> transer = new DataTransfer<PutOutput>();
            IList<string> errors = Validator.Validate(Input);
            if (errors.Count == 0)
            {
                ContactType contacttypeinput = new ContactType();
                ContactType contacttypeoutput = new ContactType();
                PutOutput output = new PutOutput();
                contacttypeinput.CopyFrom(Input);
                ContactType contacttype = _iContactTypeRepository.GetContactType(contacttypeinput.Id);
                if (contacttype!=null)
                {
                    contacttypeoutput = _iContactTypeRepository.UpdateContactType(contacttypeinput);
                    if(contacttypeoutput!=null)
                    {
                        output.CopyFrom(contacttypeoutput);
                        transer.IsSuccess = true;
                        transer.Data = output;
                    }
                    else
                    {
                        transer.IsSuccess = false;
                        transer.Errors = new string[1];
                        transer.Errors[0] = "Error: Could not update.";
                    } 
                }
                else                
                {
                    transer.IsSuccess = false;
                    transer.Errors = new string[1];
                    transer.Errors[0] = "Error: Record not found.";
                }
            }
            else
            {
                transer.IsSuccess = false;
                transer.Errors = errors.ToArray<string>();
            }
            return transer;
        }

         public DataTransfer<string> Delete(string _id)
         {
            DataTransfer<string> tranfer = new DataTransfer<string>();
            System.Int32 id=0;
            if (!string.IsNullOrEmpty(_id) && System.Int32.TryParse(_id,out id))
            {
				 bool IsDeleted = _iContactTypeRepository.DeleteContactType(id);
                if(IsDeleted)
                {
                    tranfer.IsSuccess = true;
                    tranfer.Data=IsDeleted.ToString().ToLower() ;

                }
                else
                {
                    tranfer.IsSuccess = false;
                    tranfer.Errors = new string[1];
                    tranfer.Errors[0] = "Error: No record found.";
                }             
                
            }
            else
            {
                tranfer.IsSuccess = false;
                tranfer.Errors = new string[1];
                tranfer.Errors[0] = "Error: Invalid request.";
            }
            return tranfer;
        }
	}
	
	
}
