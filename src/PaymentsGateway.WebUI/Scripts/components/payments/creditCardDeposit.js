﻿import React from 'react';

export default React.createClass({
	render:function() {
		return (
			<div className="">
				<h1>Have some fun by paying us with your: </h1>
			  <div className="container-fluid row bg-orange bg-trasparent">
				<h2 className="col-md-3">Credit Card</h2>

				<div className="col-md-9">
				  <div className="row">
					<div className="col-md-9 form-group">
						<input type="text" className="form-control" placeholder="Credit Card Number"/>
					</div>
					<div className="col-md-3 form-group">
						<input type="text" className="form-control" placeholder="CVV"/>
					</div>
					<div className="col-md-9 form-group">
					  <input type="text" className="form-control" placeholder="Name On The Card"/>
					</div>
					<div className="col-md-3 form-group">
					  <input type="email" className="form-control" placeholder="Expiration Date"/>
					</div>
				  </div>
				 
				  <div className="row">
					<div className="col-md-12 form-group">
					  <button className="btn btn-default pull-right" type="submit">Pay</button>
					</div>
				  </div> 
				</div>
			  </div>
		</div>
		);
	}
})